using System;
using Game.Core.Input;
using UnityEngine;

namespace Game.Campaign
{
  public class CampaignInputController : IDisposable
  {
    private readonly InputState _inputState;
    private readonly CampaignInputState _state;
    private readonly Camera _camera;
    
    // ReSharper disable once ParameterHidesMember
    public CampaignInputController(CampaignInputState state, InputState inputState, Camera camera)
    {
      _state = state;
      _inputState = inputState;
      _camera = camera;

      _inputState.MainMouseButtonClicked.Invoked += State_OnInputMainMouseButtonClicked;
    }

    public void Dispose()
    {
      _inputState.MainMouseButtonClicked.Invoked -= State_OnInputMainMouseButtonClicked;
    }
    
    private void State_OnInputMainMouseButtonClicked(Vector2 mousePosition)
    {
      var ray = _camera.ScreenPointToRay(mousePosition);

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