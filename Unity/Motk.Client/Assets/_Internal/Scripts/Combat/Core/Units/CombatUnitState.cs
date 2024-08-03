using System.Collections.Generic;
using com.karabaev.reactivetypes.Action;
using com.karabaev.reactivetypes.Property;
using Motk.Combat.Shared;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Client.Core.Units
{
  public class CombatUnitState
  {
    public string DescriptorId { get; }
    
    public CombatUnitIdentifier Identifier { get; }
    
    public ReactiveProperty<int> Count { get; }
    
    public ReactiveProperty<float> CurrentHp { get; }
    
    public ReactiveProperty<HexCoordinates> Position { get; }
    
    public ReactiveProperty<HexDirection> Direction { get; }

    public ReactiveAction<Stack<HexCoordinates>> MoveIntended { get; } = new();

    public CombatUnitState(string descriptorId, CombatUnitIdentifier id, int count, float currentHp,
      HexCoordinates position, HexDirection direction)
    {
      DescriptorId = descriptorId;
      Identifier = id;
      Count = new ReactiveProperty<int>(count);
      CurrentHp = new ReactiveProperty<float>(currentHp);
      Position = new ReactiveProperty<HexCoordinates>(position);
      Direction = new ReactiveProperty<HexDirection>(direction);
    }
  }
}