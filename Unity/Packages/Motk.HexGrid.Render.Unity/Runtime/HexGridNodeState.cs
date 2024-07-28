using com.karabaev.reactivetypes.Property;
using Motk.HexGrid.Core.Descriptors;

namespace Mork.HexGrid.Render.Unity
{
  public class HexGridNodeState
  {
    public HexCoordinates Coordinates { get; }
    
    public bool IsWalkable { get; }

    public ReactiveProperty<GridNodeVisualStateType> OutlineVisibility { get; }

    public HexGridNodeState(HexCoordinates coordinates, bool isWalkable)
    {
      Coordinates = coordinates;
      IsWalkable = isWalkable;
      OutlineVisibility = new ReactiveProperty<GridNodeVisualStateType>(GridNodeVisualStateType.None);
    }
  }
}