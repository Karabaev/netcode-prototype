using com.karabaev.reactivetypes.Dictionary;
using Motk.HexGrid.Core.Descriptors;

namespace Mork.HexGrid.Render.Unity
{
  public class HexGridVisualState
  {
    public ReactiveDictionary<HexCoordinates, GridNodeVisualStateType> VisibleNodes { get; } = new();
  }
}