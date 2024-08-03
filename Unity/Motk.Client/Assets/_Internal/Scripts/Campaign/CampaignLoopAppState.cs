using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Campaign.Client.InputSystem;
using Motk.Campaign.Client.Movement;
using Motk.Shared.Campaign;
using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Core.Net;
using UnityEngine;

namespace Motk.Campaign.Client
{
  [UsedImplicitly]
  public class CampaignLoopAppState : ApplicationState<DummyStateContext>
  {
    private readonly ClientMessageReceiver _messageReceiver;
    private readonly CampaignActorsState _campaignActorsState;
    private readonly LocationMovementController _locationMovementController;
    private readonly AutomatedCampaignInputController _automatedCampaignInputController;
    [UsedImplicitly] private readonly ManualCampaignInputController _manualCampaignInputController;
    
    public override UniTask EnterAsync(DummyStateContext context)
    {
      _locationMovementController.Start();
      _automatedCampaignInputController.Start();
      _messageReceiver.RegisterMessageHandler<LocationStateMessage>(Network_OnLocationStateObtained);
      _messageReceiver.RegisterMessageHandler<PlayerActorSpawnedCommand>(Network_OnActorSpawned);
      _messageReceiver.RegisterMessageHandler<PlayerActorDespawnedCommand>(Network_OnActorDespawned);
      
      Debug.Log("Campaign started...");
      
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _messageReceiver.UnregisterMessageHandler<LocationStateMessage>();
      _messageReceiver.UnregisterMessageHandler<PlayerActorSpawnedCommand>();
      _messageReceiver.UnregisterMessageHandler<PlayerActorDespawnedCommand>();
      return UniTask.CompletedTask;
    }

    private void Network_OnLocationStateObtained(LocationStateMessage message)
    {
      _campaignActorsState.Actors.Clear();
      
      foreach (var actorDto in message.Actors)
      {
        var actorState = new CampaignActorState(actorDto.Position, actorDto.EulerY);
        _campaignActorsState.Actors.Add(actorDto.PlayerId, actorState);
      }
    }

    private void Network_OnActorSpawned(PlayerActorSpawnedCommand message)
    {
      var actorState = new CampaignActorState(message.Actor.Position, message.Actor.EulerY);
      _campaignActorsState.Actors.Add(message.Actor.PlayerId, actorState);
    }

    private void Network_OnActorDespawned(PlayerActorDespawnedCommand message)
    {
      _campaignActorsState.Actors.Remove(message.PlayerId);
    }
    
    public CampaignLoopAppState(ApplicationStateMachine stateMachine, ClientMessageReceiver messageReceiver,
      CampaignActorsState campaignActorsState, LocationMovementController locationMovementController,
      AutomatedCampaignInputController automatedCampaignInputController, ManualCampaignInputController manualCampaignInputController) : base(stateMachine)
    {
      _messageReceiver = messageReceiver;
      _campaignActorsState = campaignActorsState;
      _locationMovementController = locationMovementController;
      _automatedCampaignInputController = automatedCampaignInputController;
      _manualCampaignInputController = manualCampaignInputController;
    }
  }
}