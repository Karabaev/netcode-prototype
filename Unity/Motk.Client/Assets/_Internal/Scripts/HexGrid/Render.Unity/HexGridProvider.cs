using System.Collections.Generic;
using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Mork.HexGrid.Render.Unity
{
  public class HexGridProvider : MonoBehaviour
  {
    public Motk.HexGrid.Core.HexGrid Grid { get; private set; } = null!;

    private void Awake()
    {
      Grid = new Motk.HexGrid.Core.HexGrid();

      var mapDescription = CreateMapDescriptor();
      Grid.Initialize(mapDescription);
    }

    private HexMapDescriptor CreateMapDescriptor()
    {
      var nodes = new List<HexMapNodeDescriptor>
      {
        new(new HexCoordinates(-3, 6), true),
        new(new HexCoordinates(-2, 6), true),
        new(new HexCoordinates(-1, 6), true),
        new(new HexCoordinates(-0, 6), true),
        new(new HexCoordinates(1, 6), true),
        new(new HexCoordinates(2, 6), true),
        new(new HexCoordinates(3, 6), true),

        new(new HexCoordinates(-2, 5), true),
        new(new HexCoordinates(-1, 5), true),
        new(new HexCoordinates(0, 5), true),
        new(new HexCoordinates(1, 5), true),
        new(new HexCoordinates(2, 5), true),
        new(new HexCoordinates(3, 5), true),

        new(new HexCoordinates(-2, 4), true),
        new(new HexCoordinates(-1, 4), true),
        new(new HexCoordinates(0, 4), true),
        new(new HexCoordinates(1, 4), true),
        new(new HexCoordinates(2, 4), true),
        new(new HexCoordinates(3, 4), true),
        new(new HexCoordinates(4, 4), true),

        new(new HexCoordinates(-1, 3), true),
        new(new HexCoordinates(0, 3), true),
        new(new HexCoordinates(1, 3), true),
        new(new HexCoordinates(2, 3), true),
        new(new HexCoordinates(3, 3), true),
        new(new HexCoordinates(4, 3), true),

        new(new HexCoordinates(-1, 2), true),
        new(new HexCoordinates(0, 2), true),
        new(new HexCoordinates(1, 2), true),
        new(new HexCoordinates(2, 2), true),
        new(new HexCoordinates(3, 2), true),
        new(new HexCoordinates(4, 2), true),
        new(new HexCoordinates(5, 2), true),

        new(new HexCoordinates(0, 1), true),
        new(new HexCoordinates(1, 1), true),
        new(new HexCoordinates(2, 1), true),
        new(new HexCoordinates(3, 1), true),
        new(new HexCoordinates(4, 1), true),
        new(new HexCoordinates(5, 1), true),

        new(new HexCoordinates(0, 0), true),
        new(new HexCoordinates(1, 0), true),
        new(new HexCoordinates(2, 0), true),
        new(new HexCoordinates(3, 0), true),
        new(new HexCoordinates(4, 0), true),
        new(new HexCoordinates(5, 0), true),
        new(new HexCoordinates(6, 0), true),

        new(new HexCoordinates(1, -1), true),
        new(new HexCoordinates(2, -1), true),
        new(new HexCoordinates(3, -1), true),
        new(new HexCoordinates(4, -1), true),
        new(new HexCoordinates(5, -1), true),
        new(new HexCoordinates(6, -1), true),

        new(new HexCoordinates(1, -2), true),
        new(new HexCoordinates(2, -2), true),
        new(new HexCoordinates(3, -2), true),
        new(new HexCoordinates(4, -2), true),
        new(new HexCoordinates(5, -2), true),
        new(new HexCoordinates(6, -2), true),
        new(new HexCoordinates(7, -2), true),

        new(new HexCoordinates(2, -3), true),
        new(new HexCoordinates(3, -3), true),
        new(new HexCoordinates(4, -3), true),
        new(new HexCoordinates(5, -3), true),
        new(new HexCoordinates(6, -3), true),
        new(new HexCoordinates(7, -3), true)
      };
      return new HexMapDescriptor(nodes);
    }
  }
}