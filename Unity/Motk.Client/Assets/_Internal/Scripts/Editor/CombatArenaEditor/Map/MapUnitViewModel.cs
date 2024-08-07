using UnityEngine;

namespace Motk.Editor.CombatArenaEditor.Map
{
  public class MapUnitViewModel
  {
    public readonly string Id;
    public readonly Vector3 WorldPosition;
    public readonly Vector3 Rotation;
    public readonly GameObject Prefab;

    public MapUnitViewModel(string id, Vector3 worldPosition, Vector3 rotation, GameObject prefab)
    {
      Id = id;
      WorldPosition = worldPosition;
      Rotation = rotation;
      Prefab = prefab;
    }
  }
}