using System;
using System.Linq;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Locations;
using Motk.CampaignServer.Match.Net;
using Motk.CampaignServer.Match.States;
using Motk.CampaignServer.Server.Net;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Campaign.Movement.Messages;
using Motk.Shared.Campaign.PathFinding;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Motk.CampaignServer.Match
{
  [UsedImplicitly]
  public class MatchEntryPoint : IStartable, IDisposable
  {
    private readonly LocationsRegistry _locationsRegistry;
    private readonly MatchState _matchState;
    private readonly LocationOffsetState _locationOffsetState;
    private readonly CampaignLocationState _locationState;
    private readonly NavMeshPathFindingService _pathFindingService;
    private readonly ActorMovementLogic _actorMovementLogic;
    private readonly MatchMessageSender _matchMessageSender;
    private readonly ServerMessageReceiver _messageReceiver;

    private GameObject _locationObject = null!;

    public MatchEntryPoint(LocationsRegistry locationsRegistry, MatchState matchState,
      LocationOffsetState locationOffsetState, CampaignLocationState locationState,
      NavMeshPathFindingService pathFindingService,
      ActorMovementLogic actorMovementLogic, ServerMessageReceiver messageReceiver, MatchMessageSender matchMessageSender)
    {
      _locationsRegistry = locationsRegistry;
      _matchState = matchState;
      _locationOffsetState = locationOffsetState;
      _locationState = locationState;
      _pathFindingService = pathFindingService;
      _actorMovementLogic = actorMovementLogic;
      _messageReceiver = messageReceiver;
      _matchMessageSender = matchMessageSender;
    }

    void IStartable.Start()
    {
      Debug.Log($"Match started. MatchId={_matchState.Id}");

      var locationDescriptor = _locationsRegistry.Entries[_matchState.LocationId];
      _locationObject = Object.Instantiate(locationDescriptor.Prefab);
      _matchState.Scope.transform.AddChild(_locationObject);
      _locationObject.transform.position = _locationOffsetState.Offset;

      _matchState.Users.ItemAdded += State_OnUserAdded;
      _matchState.Users.ItemRemoved += State_OnUserRemoved;
      _messageReceiver.RegisterMatchMessageHandler<StartActorMoveRequest>(_matchState.Id, Network_OnStartActorMoveRequested);
    }

    void IDisposable.Dispose()
    {
      _locationObject.DestroyObject();
      _messageReceiver.UnregisterMatchMessageHandler<StartActorMoveRequest>(_matchState.Id);
      _matchState.Users.ItemAdded -= State_OnUserAdded;
      _matchState.Users.ItemRemoved -= State_OnUserRemoved;
      Debug.Log($"Match disposed. MatchId={_matchState.Id}");
    }
    
    private void State_OnUserAdded(string key, ulong newValue) => _matchState.ConnectedClients.Add(newValue);

    private void State_OnUserRemoved(string key, ulong oldValue) => _matchState.ConnectedClients.Remove(oldValue);

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
      _matchMessageSender.Broadcast(moveStartedMessage);
    }
  }
}