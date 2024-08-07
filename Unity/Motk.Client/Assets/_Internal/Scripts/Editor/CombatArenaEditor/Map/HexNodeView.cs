using com.karabaev.utilities.unity;
using Mork.HexGrid.Render.Unity.Functions;
using Motk.HexGrid.Core.Descriptors;
using UnityEditor;
using UnityEngine;

namespace Motk.Editor.CombatArenaEditor.Map
{
  public class HexNodeView
  {
    private readonly HexCoordinates _hexCoordinates;
    private readonly HexNodeViewModel _viewModel;
    private readonly IHexGridFunctions _hexGridFunctions;

    public void OnDrawGizmos()
    {
      var color = _viewModel.IsPassable ? Color.green : Color.red;
      color.a = 0.3f;
      GizmosHelper.DrawString(_hexCoordinates.ToString(), _viewModel.WorldPosition);
      DrawSolidHex(_viewModel.WorldPosition, _viewModel.Radius, color);
      DrawHexOutline(_viewModel.WorldPosition, _viewModel.Radius, color, 5.0f);
    }
    
    // todokmo в утилиты
    private void DrawSolidHex(Vector3 center, float radius, Color color)
    {
      Handles.color = color;

      var vertices = new Vector3[6];
      for (var i = 0; i < vertices.Length; i++)
        vertices[i] = _hexGridFunctions.GetLocalCorner(i, radius) + center;

      Handles.DrawAAConvexPolygon(vertices);
    }
    
    private void DrawHexOutline(Vector3 center, float radius, Color color, float width = 1.0f)
    {
      Handles.color = color;
      var vertices = new Vector3[7];
      for (var i = 0; i < vertices.Length; i++)
        vertices[i] = _hexGridFunctions.GetLocalCorner(i, radius) + center;
      
      Handles.DrawAAPolyLine(width, vertices);
    }

    public HexNodeView(HexCoordinates hexCoordinates, HexNodeViewModel viewModel, IHexGridFunctions hexGridFunctions)
    {
      _hexCoordinates = hexCoordinates;
      _viewModel = viewModel;
      _hexGridFunctions = hexGridFunctions;
    }
  }
}