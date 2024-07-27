using com.karabaev.reactivetypes.Property;
using Motk.HexGrid.Core.Descriptors;

namespace Mork.HexGrid.Render.Unity
{
  public class HexGridNodeState
  {
    public HexCoordinates Coordinates { get; }
    
    public bool IsWalkable { get; }

    public ReactiveProperty<bool> IsHighlighted { get; }

    public HexGridNodeState(HexCoordinates coordinates, bool isWalkable)
    {
      Coordinates = coordinates;
      IsWalkable = isWalkable;
      IsHighlighted = new ReactiveProperty<bool>(false);
    }
  }
}