using System.Collections.Generic;
using com.karabaev.utilities.unity;
using com.karabaev.utilities.unity.GameKit;
using TMPro;
using UnityEngine;

namespace Motk.Client.Combat.Grid
{
  public class HexGridNodeView : GameKitComponent
  {
    [SerializeField, HideInInspector, RequireInChildren]
    private TMP_Text _positionText = null!;
    [SerializeField, HideInInspector, RequireInChildren]
    private MeshFilter _meshFilter = null!;

    private readonly List<Vector3> _vertices = new();
    private readonly List<int> _triangles = new();

    public Vector2Int Position
    {
      set => _positionText.text = $"{value.x};{value.y}";
    }

    private void Awake()
    {
      _meshFilter.mesh = new Mesh();

      Triangulate();
      
      _meshFilter.mesh.vertices = _vertices.ToArray();
      _meshFilter.mesh.triangles = _triangles.ToArray();
      _meshFilter.mesh.RecalculateNormals();
    }

    private void Triangulate()
    {
      var center = transform.localPosition;
      for (var i = 0; i < 6; i++)
      {
        AddTriangle(center, center + HexMetrics.Corners[i], center + HexMetrics.Corners[i + 1]);
      }
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
      var vertexIndex = _vertices.Count;
      _vertices.Add(v1);
      _vertices.Add(v2);
      _vertices.Add(v3);
      _triangles.Add(vertexIndex);
      _triangles.Add(vertexIndex + 1);
      _triangles.Add(vertexIndex + 2);
    }
  }
}