using System;
using com.karabaev.camera.unity.Views;
using com.karabaev.utilities.unity;
using JetBrains.Annotations;
using Motk.Client.Core.InputSystem;
using UnityEngine;
using VContainer.Unity;

namespace Motk.Client.Campaign.InputSystem
{
  [UsedImplicitly]
  public class CampaignInputController : IStartable, IDisposable
  {
    private readonly InputState _inputState;
    private readonly CampaignInputState _state;
    private GameCameraView _camera = null!;
    
    public CampaignInputController(CampaignInputState state, InputState inputState)
    {
      _state = state;
      _inputState = inputState;
    }
    
    // ReSharper disable once ParameterHidesMember
    public void Initialize(GameCameraView camera) => _camera = camera;

    void IStartable.Start()
    {
      _inputState.MainMouseButtonClicked.Invoked += State_OnInputMainMouseButtonClicked;
    }
    
    void IDisposable.Dispose()
    {
      _inputState.MainMouseButtonClicked.Invoked -= State_OnInputMainMouseButtonClicked;
    }
    
    private void State_OnInputMainMouseButtonClicked(Vector2 mousePosition)
    {
      var ray = _camera.RequireComponent<Camera>().ScreenPointToRay(mousePosition);

      if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, LayerMask.GetMask("InteractiveObject")))
      {
        // var interactiveObject = hitInfo.collider.RequireComponent<IInteractiveObjectView>();
        // _campaignInputState.InteractiveObjectClicked.Invoke(interactiveObject.Info);
        return;
      }

      if (Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.GetMask("Ground")))
      {
        _state.GroundClicked.Invoke(hitInfo.point);
      }
    }
  }
}