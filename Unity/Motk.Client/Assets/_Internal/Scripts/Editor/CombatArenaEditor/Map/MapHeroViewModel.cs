using UnityEngine;

namespace Motk.Editor.CombatArenaEditor.Map
{
  public class MapHeroViewModel
  {
    public readonly string Id;
    public readonly byte TeamIndex;
    public readonly GameObject Prefab;

    public MapHeroViewModel(string id, byte teamIndex, GameObject prefab)
    {
      Id = id;
      TeamIndex = teamIndex;
      Prefab = prefab;
    }
  }
}