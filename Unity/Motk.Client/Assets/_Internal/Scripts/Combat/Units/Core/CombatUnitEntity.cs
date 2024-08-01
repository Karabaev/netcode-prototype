using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mork.HexGrid.Render.Unity;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;
using PrimeTween;
using UnityEngine;

namespace Motk.Client.Combat.Units.Core
{
  public class CombatUnitEntity : IDisposable
  {
    private const float HexToHexMoveDuration = 0.3f;
    private const float AngularSpeed = 720f;
    
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
      _view.Rotation = _state.Direction.Value.ToWorld();
    }

    public void Dispose()
    {
      _state.MoveIntended.Invoked -= State_OnMoveIntended;
    }

    private void State_OnMoveIntended(Stack<HexCoordinates> path)
    {
      MoveAsync(path).Forget();
    }

    private async UniTaskVoid MoveAsync(Stack<HexCoordinates> path)
    {
      foreach (var nextPoint in path)
      {
        var currentPosition = _state.Position.Value;
        var nextDirection = HexRenderUtils.GetDirection(currentPosition, nextPoint);
        await MoveAsync(currentPosition, nextPoint, nextDirection, HexToHexMoveDuration);
        _state.Position.Value = nextPoint;
        _state.Direction.Value = nextDirection;
      }
    }
    
    private async UniTask MoveAsync(HexCoordinates origin, HexCoordinates destination, HexDirection nextDirection,
      float duration)
    {
      // todokmo нужен механизм, который бы снаппил по Y
      var originWorld = origin.ToWorld(0.0f);
      var destinationWorld = destination.ToWorld(0.0f);

      var startRotation = _state.Direction.Value.ToWorld();
      var endRotation = nextDirection.ToWorld();
      var rotationDuration = Quaternion.Angle(startRotation, endRotation) / AngularSpeed;
      Tween.Custom(_view, startRotation, endRotation, rotationDuration, OnRotationTick, Ease.Linear).ToUniTask().Forget();
      await Tween.Custom(_view, originWorld, destinationWorld, duration, OnMoveTick, Ease.Linear);

      void OnMoveTick(ICombatUnitView view, Vector3 newPosition) => view.Position = newPosition;
      void OnRotationTick(ICombatUnitView view, Quaternion newRotation) => view.Rotation = newRotation;
    }
  }
}