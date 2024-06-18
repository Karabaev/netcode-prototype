using System;
using System.Collections.Generic;
using com.karabaev.camera.unity.States;
using com.karabaev.utilities.unity;
using JetBrains.Annotations;
using Motk.Client.Campaign.Actors.Services;
using Motk.Client.Campaign.Actors.Views;
using Motk.Client.Campaign.CameraSystem;
using Motk.Client.Campaign.Player;
using Motk.Shared.Campaign.Actors.States;
using VContainer.Unity;

namespace Motk.Client.Campaign.Actors.Controllers
{
  [UsedImplicitly]
  public class LocationActorsController : IInitializable, IDisposable
  {
    private readonly CampaignActorViewFactory _actorViewFactory;
    private readonly CampaignActorsState _state;
    private readonly CurrentPlayerState _playerState;
    private readonly GameCameraState _gameCameraState;

    private readonly Dictionary<ulong, CampaignActorView> _actorViews = new();

    public LocationActorsController(CampaignActorsState state, CampaignActorViewFactory actorViewFactory,
      CurrentPlayerState playerState, GameCameraState gameCameraState)
    {
      _state = state;
      _actorViewFactory = actorViewFactory;
      _playerState = playerState;
      _gameCameraState = gameCameraState;

    }

    void IInitializable.Initialize()
    {
      _state.Actors.ItemAdded += State_OnActorAdded;
      _state.Actors.ItemRemoved += State_OnActorRemoved;
      _state.Actors.Cleaned += State_OnActorsCleaned;
    }
    
    void IDisposable.Dispose()
    {
      _state.Actors.ItemAdded -= State_OnActorAdded;
      _state.Actors.ItemRemoved -= State_OnActorRemoved;
      _state.Actors.Cleaned -= State_OnActorsCleaned;
      
      foreach (var (_, actorView) in _actorViews)
        actorView.DestroyObject();
    }

    private void State_OnActorAdded(ulong playerId, CampaignActorState newActor)
    {
      var view = _actorViewFactory.Create("default", newActor); 
      _actorViews.Add(playerId, view);

      if (_playerState.ClientId != playerId)
        return;

      _gameCameraState.Target.Value = new CurrentPlayerActorCameraTarget(newActor);
    }

    private void State_OnActorRemoved(ulong clientId, CampaignActorState oldActor)
    {
      _actorViews.Remove(clientId, out var view);
      view.DestroyObject();
    }

    private void State_OnActorsCleaned()
    {
      foreach (var (_, actorView) in _actorViews)
        actorView.DestroyObject();

      _actorViews.Clear();
    }
  }
}