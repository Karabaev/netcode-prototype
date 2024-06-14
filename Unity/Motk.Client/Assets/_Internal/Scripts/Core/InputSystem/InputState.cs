using com.karabaev.reactivetypes.Action;
using UnityEngine;

namespace Motk.Client.Core.InputSystem
{
  public class InputState
  {
    public ReactiveAction<Vector2> MainMouseButtonClicked { get; } = new();
  }
}