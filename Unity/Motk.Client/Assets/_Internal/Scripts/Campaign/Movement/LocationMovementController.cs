using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Campaign.Client.InputSystem;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Campaign.Movement.Messages;
using Motk.Shared.Core.Net;
using UnityEngine;

namespace Motk.Campaign.Client.Movement
{
  // todokmo логику перенести в стейт?
  [UsedImplicitly]
  public class LocationMovementController : IDisposable
  {
    private readonly ActorMovementLogic _actorMovementLogic;
    private readonly CampaignActorsState _actorsState;
    private readonly ClientMessageReceiver _messageReceiver;
    private readonly ClientMessageSender _messageSender;
    private readonly CampaignInputState _campaignInputState;

    public LocationMovementController(ActorMovementLogic actorMovementLogic,
      CampaignActorsState actorsState, ClientMessageReceiver messageReceiver, ClientMessageSender messageSender,
      CampaignInputState campaignInputState)
    {
      _actorMovementLogic = actorMovementLogic;
      _actorsState = actorsState;
      _messageReceiver = messageReceiver;
      _messageSender = messageSender;
      _campaignInputState = campaignInputState;
    }

    public void Start()
    {
      _campaignInputState.GroundClicked.Invoked += State_OnGroundClicked;
      _messageReceiver.RegisterMessageHandler<ActorMoveStartedCommand>(Network_OnActorMoveStarted);
    }

    void IDisposable.Dispose()
    {
      _campaignInputState.GroundClicked.Invoked -= State_OnGroundClicked;
      _messageReceiver.UnregisterMessageHandler<ActorMoveStartedCommand>();
    }

    private void Network_OnActorMoveStarted(ActorMoveStartedCommand message)
    {
      var actorState = _actorsState.Actors.Require(message.PlayerId);
      _actorMovementLogic.MoveAsync(actorState, message.Path).Forget();
    }

    private void State_OnGroundClicked(Vector3 point)
    {
      var message = new StartActorMoveRequest { Destination = point };
      _messageSender.Send(message);
    }
  }
}