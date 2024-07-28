using System.Collections.Generic;
using com.karabaev.utilities.unity;
using com.karabaev.utilities.unity.GameKit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mork.HexGrid.Render.Unity
{
  public class HexGridNodeView : GameKitComponent
  {
    [SerializeField, HideInInspector, RequireInChildren]
    private TMP_Text _positionText = null!;
    [SerializeField, HideInInspector, RequireInChildren]
    private MeshFilter _meshFilter = null!;
    [SerializeField, HideInInspector, RequireInChildren]
    private Renderer _renderer = null!;
    [SerializeField, HideInInspector, Require]
    private MeshCollider _meshCollider = null!;
    [SerializeField, HideInInspector, RequireInChild("Outline")]
    private Image _outline = null!;

    [SerializeField]
    private Color _walkableColor = Color.white;
    [SerializeField]
    private Color _notWalkableColor = Color.red;
    
    [SerializeField]
    private Color _availableForMoveColor = Color.white;
    [SerializeField]
    private Color _availableForAttackColor = Color.red;
    [SerializeField]
    private Color _availableForAbilityColor = Color.blue;
    
    private readonly List<Vector3> _vertices = new();
    private readonly List<int> _triangles = new();

    private HexGridNodeState _state = null!;

    private void Awake()
    {
      _meshFilter.mesh = new Mesh();

      Triangulate();
      
      _meshFilter.mesh.vertices = _vertices.ToArray();
      _meshFilter.mesh.triangles = _triangles.ToArray();
      _meshFilter.mesh.RecalculateNormals();

      _meshCollider.sharedMesh = _meshFilter.mesh;
    }

    public void Construct(HexGridNodeState state)
    {
      _state = state;
      _state.OutlineVisibility.Changed += State_OnOutlineVisibilityChanged;

      _positionText.text = _state.Coordinates.ToString();
      _renderer.material.color = _state.IsWalkable ? _walkableColor : _notWalkableColor;

      State_OnOutlineVisibilityChanged(GridNodeVisualStateType.None, _state.OutlineVisibility.Value);
    }

    private void OnDestroy()
    {
      if (_state == null!)
        return;
      
      _state.OutlineVisibility.Changed -= State_OnOutlineVisibilityChanged;
    }

    private void State_OnOutlineVisibilityChanged(GridNodeVisualStateType oldValue, GridNodeVisualStateType newValue)
    {
      if (newValue is GridNodeVisualStateType.None)
      {
        _outline.SetActive(false);
        return;
      }
      
      var color = newValue switch
      {
        GridNodeVisualStateType.Move => _availableForMoveColor,
        GridNodeVisualStateType.Attack => _availableForAttackColor,
        GridNodeVisualStateType.Ability => _availableForAbilityColor,
      };

      _outline.SetActive(true);
      _outline.color = color;
    }

    private void Triangulate()
    {
      var center = transform.localPosition;
      for (var i = 0; i < 6; i++)
      {
        AddTriangle(center, center + HexRenderUtils.Corners[i], center + HexRenderUtils.Corners[i + 1]);
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