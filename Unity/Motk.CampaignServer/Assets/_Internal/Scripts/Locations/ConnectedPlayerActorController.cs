using System;
using System.Linq;
using JetBrains.Annotations;
using Motk.CampaignServer.Match.Net;
using Motk.CampaignServer.Match.States;
using Motk.Shared.Campaign;
using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Locations;
using VContainer.Unity;

namespace Motk.CampaignServer.Locations
{
  [UsedImplicitly]
  public class ConnectedPlayerActorController : IStartable, IDisposable
  {
    private readonly MatchState _matchState;
    private readonly CampaignLocationState _locationState;
    private readonly LocationOffsetState _locationOffsetState;
    private readonly MatchMessageSender _matchMessageSender;

    public ConnectedPlayerActorController(MatchState matchState, CampaignLocationState locationState,
      LocationOffsetState locationOffsetState, MatchMessageSender matchMessageSender)
    {
      _matchState = matchState;
      _locationState = locationState;
      _locationOffsetState = locationOffsetState;
      _matchMessageSender = matchMessageSender;
    }

    void IStartable.Start()
    {
      _matchState.Users.ItemAdded += State_OnUserAdded;
      _matchState.Users.ItemRemoved += State_OnUserRemoved;
    }
    
    void IDisposable.Dispose()
    {
      _matchState.Users.ItemAdded -= State_OnUserAdded;
      _matchState.Users.ItemRemoved -= State_OnUserRemoved;
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
  }
}