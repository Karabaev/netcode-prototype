using System;
using System.Linq;
using Motk.CampaignServer.Matches.States;
using Motk.Shared.Campaign;
using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Core.Net;
using Motk.Shared.Locations;

namespace Motk.CampaignServer.Locations
{
  public class ConnectedPlayerController : IDisposable
  {
    private readonly MatchState _matchState;
    private readonly ServerMessageSender _messageSender;
    private readonly CampaignLocationState _locationState;

    public ConnectedPlayerController(MatchState matchState, ServerMessageSender messageSender, CampaignLocationState locationState)
    {
      _matchState = matchState;
      _messageSender = messageSender;
      _locationState = locationState;
      _matchState.Users.ItemAdded += State_OnUserAdded;
      _matchState.Users.ItemRemoved += State_OnUserRemoved;
    }

    public void Dispose()
    {
      _matchState.Users.ItemAdded -= State_OnUserAdded;
      _matchState.Users.ItemRemoved -= State_OnUserRemoved;
    }

    private void State_OnUserAdded(string userSecret, ulong clientId)
    {
      // создать перса
      // отправить сообщение о состоянии локации
      // бродкаст всем остальным
      
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
      _locationState.Actors.Add(clientId, newActorState);

      _messageSender.Broadcast(new PlayerActorSpawnedCommand
      {
        Actor = new CampaignActorDto
        {
          PlayerId = clientId,
          Position = newActorState.Position.Value,
          Rotation = newActorState.Rotation.Value
        }
      });
    }

    private void State_OnUserRemoved(string userSecret, ulong removedClientId)
    {
      
    }
  }
}