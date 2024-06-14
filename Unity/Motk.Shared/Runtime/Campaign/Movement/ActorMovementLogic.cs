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
      var moveDuration = Vector3.Distance(startPosition, destination) / 5;
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
      var targetRotation = Quaternion.LookRotation(direction);
      var targetAngle = Quaternion.Angle(actor.Rotation.Value, targetRotation);

      if(Mathf.Approximately(targetAngle, 0))
        return UniTask.CompletedTask;

      var rotationDuration = targetAngle / 720.0f;

      return Tween
        .Custom(actor, actor.Rotation.Value, targetRotation, rotationDuration, OnTick, Ease.Linear)
        .ToUniTask();

      void OnTick(CampaignActorState state, Quaternion rotation) => state.Rotation.Value = rotation;
    }
  }
}