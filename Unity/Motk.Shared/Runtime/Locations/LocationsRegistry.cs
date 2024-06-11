using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Motk.Shared.Locations
{
  [CreateAssetMenu(menuName = "LocationsRegistry")]
  public class LocationsRegistry : ScriptableObject
  {
    [SerializeField]
    private List<LocationDescriptor> _locations = null!;

    public IReadOnlyDictionary<string, LocationDescriptor> Locations => 
      _locations.ToDictionary(l => l.Id, l => l);
  }
}