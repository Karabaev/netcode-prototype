using com.karabaev.descriptors.abstractions.Initialization;
using com.karabaev.descriptors.unity;
using UnityEngine;

namespace Motk.Client.Units
{
  [DescriptorSource("DR_UnitVisual", typeof(ResourcesDescriptorSourceProvider))]
  [CreateAssetMenu(menuName = "Motk/UnitVisualRegistry")]
  public class UnitVisualRegistrySource : ScriptableObjectDescriptorRegistrySource<string, UnitVisualDescriptor> { }
}