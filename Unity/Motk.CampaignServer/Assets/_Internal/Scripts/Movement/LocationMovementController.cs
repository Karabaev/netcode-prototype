using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Motk.CampaignServer.Locations;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Campaign.Movement.Messages;
using Motk.Shared.Campaign.PathFinding;
using Motk.Shared.Core.Net;
using Motk.Shared.Locations;
using VContainer.Unity;

namespace Motk.CampaignServer.Movement
{
  public class LocationMovementController : IStartable, IDisposable
  {
    private readonly CampaignLocationState _locationState;
    private readonly ServerMessageReceiver _messageReceiver;
    private readonly ServerMessageSender _messageSender;
    private readonly ActorMovementLogic _actorMovementLogic;
    private readonly NavMeshPathFindingService _pathFindingService;
    private readonly LocationOffsetState _locationOffsetState;

    void IStartable.Start()
    {
      _messageReceiver.RegisterMessageHandler<StartActorMoveRequest>(Network_OnStartActorMoveRequested);
    }

    void IDisposable.Dispose()
    {
      _messageReceiver.UnregisterMessageHandler<StartActorMoveRequest>();
    }

    private void Network_OnStartActorMoveRequested(ulong senderId, StartActorMoveRequest message)
    {
      var actorState = _locationState.Actors.Require(senderId);
      var destination = _locationOffsetState.Offset + message.Destination;
      var path = _pathFindingService.FindPath(actorState.Position.Value, destination);

      if (path.Length == 0)
        return;
      
      path = path.Skip(1).ToArray();
      _actorMovementLogic.MoveAsync(actorState, path).Forget();

      var moveStartedMessage = new ActorMoveStartedCommand
      {
        PlayerId = senderId,
        Path = path.Select(p => p - _locationOffsetState.Offset).ToArray()
      };
      _messageSender.Broadcast(moveStartedMessage);
    }

    public LocationMovementController(CampaignLocationState locationState, ServerMessageReceiver messageReceiver,
      ServerMessageSender messageSender, ActorMovementLogic actorMovementLogic, NavMeshPathFindingService pathFindingService,
      LocationOffsetState locationOffsetState)
    {
      _locationState = locationState;
      _messageReceiver = messageReceiver;
      _messageSender = messageSender;
      _actorMovementLogic = actorMovementLogic;
      _pathFindingService = pathFindingService;
      _locationOffsetState = locationOffsetState;
    }
  }
}