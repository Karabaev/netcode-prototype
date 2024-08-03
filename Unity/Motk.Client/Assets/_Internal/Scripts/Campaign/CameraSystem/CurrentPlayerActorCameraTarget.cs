using System;
using com.karabaev.camera.unity.Views;
using Motk.Shared.Campaign.Actors.States;
using UnityEngine;

namespace Motk.Campaign.Client.CameraSystem
{
  public class CurrentPlayerActorCameraTarget : ICameraTarget
  {
    private readonly CampaignActorState _actor;
    
    public Func<Vector3> PositionFunc { get; }

    public CurrentPlayerActorCameraTarget(CampaignActorState actor)
    {
      _actor = actor;
      PositionFunc = GetActorPosition;
    }

    private Vector3 GetActorPosition() => _actor.Position.Value;
  }
}