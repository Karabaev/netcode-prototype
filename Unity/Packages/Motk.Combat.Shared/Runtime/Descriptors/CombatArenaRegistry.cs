using Motk.Descriptors.Registry;

namespace Motk.Combat.Shared.Descriptors
{
  public class CombatArenaRegistry : DescriptorRegistry<string, CombatArenaDescriptor>
  {
    protected override string Category => "CombatArenas";
  }
}