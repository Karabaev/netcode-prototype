using System;
using System.Linq;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Match.Net;
using Motk.CampaignServer.Match.States;
using Motk.CampaignServer.Server.Net;
using Motk.Shared.Campaign;
using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Campaign.Movement.Messages;
using Motk.Shared.Campaign.PathFinding;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Motk.CampaignServer.Locations
{
  [UsedImplicitly]
  public class LocationController : IStartable, IDisposable
  {
    private readonly MatchState _matchState;
    private readonly LocationOffsetState _locationOffsetState;
    private readonly MatchMessageSender _matchMessageSender;
    private readonly CampaignLocationState _locationState;
    private readonly NavMeshPathFindingService _pathFindingService;
    private readonly ActorMovementLogic _actorMovementLogic;
    private readonly ServerMessageReceiver _messageReceiver;
    private readonly LocationsRegistry _locationsRegistry;

    private GameObject _locationObject = null!;

    public LocationController(MatchState matchState, CampaignLocationState locationState,
      LocationOffsetState locationOffsetState, MatchMessageSender matchMessageSender,
      NavMeshPathFindingService pathFindingService, ActorMovementLogic actorMovementLogic,
      ServerMessageReceiver messageReceiver, LocationsRegistry locationsRegistry)
    {
      _matchState = matchState;
      _locationState = locationState;
      _locationOffsetState = locationOffsetState;
      _matchMessageSender = matchMessageSender;
      _pathFindingService = pathFindingService;
      _actorMovementLogic = actorMovementLogic;
      _messageReceiver = messageReceiver;
      _locationsRegistry = locationsRegistry;
    }

    void IStartable.Start()
    {
      var locationDescriptor = _locationsRegistry.Entries[_matchState.LocationId];
      _locationObject = Object.Instantiate(locationDescriptor.Prefab, _matchState.Scope.transform);
      _locationObject.transform.position = _locationOffsetState.Offset;

      _matchState.Users.ItemAdded += State_OnUserAdded;
      _matchState.Users.ItemRemoved += State_OnUserRemoved;
      
      _messageReceiver.RegisterMatchMessageHandler<StartActorMoveRequest>(_matchState.Id, Network_OnStartActorMoveRequested);
    }
    
    void IDisposable.Dispose()
    {
      _locationObject.DestroyObject();
      _matchState.Users.ItemAdded -= State_OnUserAdded;
      _matchState.Users.ItemRemoved -= State_OnUserRemoved;
      _messageReceiver.UnregisterMatchMessageHandler<StartActorMoveRequest>(_matchState.Id);
    }

    private void State_OnUserAdded(string userSecret, ulong clientId)
    {
      var locationStateMessage = new LocationStateMessage
      {
        Actors = _locationState.Actors.Select(a => new CampaignActorDto
        {
          PlayerId = a.Key,
          Position = a.Value.Position.Value - _locationOffsetState.Offset,
          Rotation = a.Value.Rotation.Value
        }).ToArray()
      };
      _matchMessageSender.Send(locationStateMessage, clientId);

      var newActorState = new CampaignActorState();
      newActorState.Position.Value += _locationOffsetState.Offset;
      
      _locationState.Actors.Add(clientId, newActorState);

      _matchMessageSender.Broadcast(new PlayerActorSpawnedCommand
      {
        Actor = new CampaignActorDto
        {
          PlayerId = clientId,
          Position = newActorState.Position.Value - _locationOffsetState.Offset,
          Rotation = newActorState.Rotation.Value
        }
      });
    }

    private void State_OnUserRemoved(string userSecret, ulong removedClientId)
    {
      _locationState.Actors.Remove(removedClientId);
      _matchMessageSender.Broadcast(new PlayerActorDespawnedCommand { PlayerId = removedClientId });
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
      _matchMessageSender.Broadcast(moveStartedMessage);
    }
  }
}