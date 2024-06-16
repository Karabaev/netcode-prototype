using System;
using System.Linq;
using JetBrains.Annotations;
using Motk.CampaignServer.Matches.States;
using Motk.Shared.Campaign;
using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Core.Net;
using Motk.Shared.Locations;
using VContainer.Unity;

namespace Motk.CampaignServer.Locations
{
  [UsedImplicitly]
  public class ConnectedPlayerController : IStartable, IDisposable
  {
    private readonly MatchState _matchState;
    private readonly ServerMessageSender _messageSender;
    private readonly CampaignLocationState _locationState;
    private readonly LocationOffsetState _locationOffsetState;

    public ConnectedPlayerController(MatchState matchState, ServerMessageSender messageSender,
      CampaignLocationState locationState, LocationOffsetState locationOffsetState)
    {
      _matchState = matchState;
      _messageSender = messageSender;
      _locationState = locationState;
      _locationOffsetState = locationOffsetState;
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
          Position = a.Value.Position.Value,
          Rotation = a.Value.Rotation.Value
        }).ToArray()
      };
      _messageSender.Send(locationStateMessage, clientId);

      var newActorState = new CampaignActorState();
      newActorState.Position.Value += _locationOffsetState.Offset;
      
      _locationState.Actors.Add(clientId, newActorState);

      _messageSender.Broadcast(new PlayerActorSpawnedCommand
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
      _messageSender.Broadcast(new PlayerActorDespawnedCommand { PlayerId = removedClientId });
    }
  }
}