using System;
using com.karabaev.camera.unity.Views;
using com.karabaev.utilities.unity;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity;
using Mork.HexGrid.Render.Unity.Functions;
using Motk.Client.Core.InputSystem;
using UnityEngine;
using VContainer.Unity;

namespace Motk.Combat.Client.Core.InputSystem
{
  [UsedImplicitly]
  public class CombatInputController : ITickable, IDisposable
  {
    private readonly InputState _inputState;
    private readonly CombatInputState _state;
    private readonly GameCameraView _camera;
    private readonly IHexGridFunctions _hexGridFunctions;
    
    public CombatInputController(CombatInputState state, InputState inputState, GameCameraView camera,
      IHexGridFunctions hexGridFunctions)
    {
      _state = state;
      _inputState = inputState;
      _camera = camera;
      _hexGridFunctions = hexGridFunctions;
    }

    public void Start()
    {
      _inputState.MainMouseButtonClicked.Invoked += State_OnInputMainMouseButtonClicked;
    }
    
    void IDisposable.Dispose()
    {
      _inputState.MainMouseButtonClicked.Invoked -= State_OnInputMainMouseButtonClicked;
    }

    void ITickable.Tick()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        _state.ReadyToBattleRaised.Invoke();
      }
    }
    
    private void State_OnInputMainMouseButtonClicked(Vector2 mousePosition)
    {
      var ray = _camera.RequireComponent<Camera>().ScreenPointToRay(mousePosition);
      
      if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, LayerMask.GetMask("Ground")))
      {
        _hexGridFunctions.ToHexCoordinates(hitInfo.point, HexRenderUtils.OuterRadius);
      }
    }
  }
}