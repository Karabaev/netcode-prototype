using System;
using System.Collections.Generic;
using com.karabaev.camera.unity.States;
using com.karabaev.utilities.unity;
using JetBrains.Annotations;
using Motk.Client.Campaign.Camera;
using Motk.Client.Core;
using Motk.Shared.Campaign.Actors;
using Motk.Shared.Campaign.Actors.States;

namespace Motk.Client.Campaign.Actors
{
  [UsedImplicitly]
  public class CampaignActorViewsController : IDisposable
  {
    private readonly CampaignActorViewFactory _actorViewFactory;
    private readonly CampaignActorsState _state;
    private readonly CurrentPlayerClientState _playerState;
    private readonly GameCameraState _gameCameraState;

    private readonly Dictionary<ulong, CampaignActorView> _actorViews = new();

    public CampaignActorViewsController(CampaignActorsState state, CampaignActorViewFactory actorViewFactory,
      CurrentPlayerClientState playerState, GameCameraState gameCameraState)
    {
      _state = state;
      _actorViewFactory = actorViewFactory;
      _playerState = playerState;
      _gameCameraState = gameCameraState;
      _state.Actors.ItemAdded += State_OnActorAdded;
      _state.Actors.ItemRemoved += State_OnActorRemoved;
    }

    public void Dispose()
    {
      _state.Actors.ItemAdded -= State_OnActorAdded;
      _state.Actors.ItemRemoved -= State_OnActorRemoved;
    }
    
    private void State_OnActorAdded(ulong playerId, CampaignActorState newActor)
    {
      var view = _actorViewFactory.Create("default", newActor); 
      _actorViews.Add(playerId, view);

      if (_playerState.ClientId != playerId)
        return;

      _gameCameraState.Target.Value = new CurrentPlayerActorCameraTarget(newActor);
    }

    private void State_OnActorRemoved(ulong playerId, CampaignActorState oldActor)
    {
      _actorViews.Remove(playerId, out var view);
      view.DestroyObject();
    }
  }
}