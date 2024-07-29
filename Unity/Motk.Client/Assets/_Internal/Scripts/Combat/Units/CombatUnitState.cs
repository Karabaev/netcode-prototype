using com.karabaev.reactivetypes.Property;
using Motk.Combat.Shared;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Client.Combat.Units
{
  public class CombatUnitState
  {
    public string DescriptorId { get; }
    
    public CombatUnitIdentifier Identifier { get; }
    
    public ReactiveProperty<int> Count { get; }
    
    public ReactiveProperty<float> CurrentHp { get; }
    
    public ReactiveProperty<HexCoordinates> Position { get; }

    public CombatUnitState(string descriptorId, CombatUnitIdentifier id, int count, float currentHp, HexCoordinates position)
    {
      DescriptorId = descriptorId;
      Identifier = id;
      Count = new ReactiveProperty<int>(count);
      CurrentHp = new ReactiveProperty<float>(currentHp);
      Position = new ReactiveProperty<HexCoordinates>(position);
    }
  }
}