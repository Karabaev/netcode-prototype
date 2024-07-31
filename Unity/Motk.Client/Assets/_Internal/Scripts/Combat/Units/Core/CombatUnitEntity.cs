using System;
using Cysharp.Threading.Tasks;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Combat.Grid;
using Motk.HexGrid.Core.Descriptors;
using PrimeTween;
using UnityEngine;

namespace Motk.Client.Combat.Units.Core
{
  public class CombatUnitEntity : IDisposable
  {
    private const float HexToHexMoveDuration = 0.3f;
    
    private readonly CombatUnitState _state;
    private readonly ICombatUnitView _view;

    public CombatUnitEntity(CombatUnitState state, ICombatUnitView view)
    {
      _state = state;
      _view = view;
    }

    public void Start()
    {
      _state.MoveIntended.Invoked += State_OnMoveIntended;
      _view.Position = _state.Position.Value.ToWorld(0.0f);
    }

    public void Dispose()
    {
      _state.MoveIntended.Invoked -= State_OnMoveIntended;
    }

    private void State_OnMoveIntended(HexCoordinates[] path)
    {
      MoveAsync(path).Forget();
    }

    private async UniTaskVoid MoveAsync(HexCoordinates[] path)
    {
      foreach (var nextPoint in path)
      {
        var currentPosition = _state.Position.Value;
        await MoveAsync(currentPosition, nextPoint, HexToHexMoveDuration);
        _state.Position.Value = nextPoint;
      }
    }
    
    private async UniTask MoveAsync(HexCoordinates origin, HexCoordinates destination, float duration)
    {
      // todokmo нужен механизм, который бы снаппил по Y
      var originWorld = origin.ToWorld(0.0f);
      var destinationWorld = destination.ToWorld(0.0f);

      await Tween.Custom(_view, originWorld, destinationWorld, duration, OnTick);

      void OnTick(ICombatUnitView view, Vector3 newPosition) => view.Position = newPosition;
    }
  }
}