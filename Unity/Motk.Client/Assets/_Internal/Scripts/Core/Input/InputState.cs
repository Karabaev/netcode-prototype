﻿using com.karabaev.reactivetypes.Action;
using UnityEngine;

namespace Motk.Client.Core.Input
{
  public class InputState
  {
    public ReactiveAction<Vector2> MainMouseButtonClicked { get; } = new();
  }
}