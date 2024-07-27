using System;
using com.karabaev.camera.unity.Views;
using com.karabaev.utilities.unity;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Core.InputSystem;
using UnityEngine;

namespace Motk.Client.Combat.InputSystem
{
  [UsedImplicitly]
  public class CombatInputController : IDisposable
  {
    private readonly InputState _inputState;
    private readonly CombatInputState _state;
    private readonly GameCameraView _camera;
    
    public CombatInputController(CombatInputState state, InputState inputState, GameCameraView camera)
    {
      _state = state;
      _inputState = inputState;
      _camera = camera;
      _inputState.MainMouseButtonClicked.Invoked += State_OnInputMainMouseButtonClicked;
    }
    
    void IDisposable.Dispose()
    {
      _inputState.MainMouseButtonClicked.Invoked -= State_OnInputMainMouseButtonClicked;
    }
    
    private void State_OnInputMainMouseButtonClicked(Vector2 mousePosition)
    {
      var ray = _camera.RequireComponent<Camera>().ScreenPointToRay(mousePosition);
      
      if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, LayerMask.GetMask("Ground")))
      {
        _state.HexClicked.Invoke(hitInfo.point.ToAxialCoordinates());
      }
    }
  }
}