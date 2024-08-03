using com.karabaev.descriptors.abstractions.Initialization;
using com.karabaev.descriptors.unity;
using UnityEngine;

namespace Motk.Combat.Client.Render.Units.Descriptors
{
  [DescriptorSource("DR_UnitVisual", typeof(ResourcesDescriptorSourceProvider))]
  [CreateAssetMenu(menuName = "Motk/UnitVisualRegistry")]
  public class UnitVisualRegistrySource : ScriptableObjectDescriptorRegistrySource<string, UnitVisualDescriptor> { }
}