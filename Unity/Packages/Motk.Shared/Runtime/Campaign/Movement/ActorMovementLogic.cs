using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Shared.Campaign.Actors.States;
using PrimeTween;
using UnityEngine;

namespace Motk.Shared.Campaign.Movement
{
  [UsedImplicitly]
  public class ActorMovementLogic
  {
    private const float MoveSpeed = 10.0f;
    private const float RotationSpeed = 720.0f;
    
    public async UniTask MoveAsync(CampaignActorState actor, IReadOnlyList<Vector3> path)
    {
      foreach(var point in path)
      {
        RotateActorAsync(actor, actor.Position.Value, point).Forget();
        await MoveActorAsync(actor, actor.Position.Value, point);
      }
    }
    
    private UniTask MoveActorAsync(CampaignActorState actor, Vector3 startPosition, Vector3 destination)
    {
      var moveDuration = Vector3.Distance(startPosition, destination) / MoveSpeed;
      return Tween
        .Custom(actor, startPosition, destination, moveDuration, OnTick, Ease.Linear)
        .ToUniTask();
      void OnTick(CampaignActorState state, Vector3 position) => state.Position.Value = position;
    }
    
    private static UniTask RotateActorAsync(CampaignActorState actor, Vector3 from, Vector3 to)
    {
      to.y = 0.0f;
      from.y = 0.0f;
      var direction = Vector3.Normalize(to - from);
      var targetRotationY = Quaternion.LookRotation(direction).eulerAngles.y;
      var angularDistance = actor.EulerY.Value - targetRotationY;

      if(Mathf.Approximately(angularDistance, 0))
        return UniTask.CompletedTask;

      var rotationDuration = angularDistance / RotationSpeed;

      return Tween
        .Custom(actor, actor.EulerY.Value, targetRotationY, rotationDuration, OnTick, Ease.Linear)
        .ToUniTask();

      void OnTick(CampaignActorState state, float eulerY) => state.EulerY.Value = eulerY;
    }
  }
}