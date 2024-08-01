using com.karabaev.camera.unity.States;
using com.karabaev.reactivetypes.Action;
using JetBrains.Annotations;
using UnityEngine;

namespace Motk.Client.Core.InputSystem
{
  [UsedImplicitly]
  public class InputState : ICameraInputState
  {
    public ReactiveAction<Vector2> MainMouseButtonClicked { get; } = new();
    
    public float MouseWheelAxis { get; set; }
    
    public Vector2 AuxMouseButtonDragAxis { get; set; }
    
    public ReactiveAction WaitRaised { get; } = new();

    public ReactiveAction DefendRaised { get; } = new();
  }
}