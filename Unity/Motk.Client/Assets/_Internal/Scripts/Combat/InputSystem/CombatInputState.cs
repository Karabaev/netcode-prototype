using com.karabaev.reactivetypes.Action;
using JetBrains.Annotations;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Client.Combat.InputSystem
{
  [UsedImplicitly]
  public class CombatInputState
  {
    public ReactiveAction<HexCoordinates> HexClicked { get; } = new();
  }
}