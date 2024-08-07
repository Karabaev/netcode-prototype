using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Mork.HexGrid.Render.Unity.Functions
{
  public interface IHexGridFunctions
  {
    Vector3 GetLocalCorner(int index, float radius);
    Vector3 ToLocal(HexCoordinates coordinates, float radius);
    HexCoordinates ToHexCoordinates(Vector3 position, float radius);
  }
}