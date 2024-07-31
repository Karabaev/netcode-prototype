using Cysharp.Threading.Tasks;
using Motk.Client.Combat.Units.Core;
using Motk.Client.Combat.Units.Core.Services;
using Motk.Client.Units;
using UnityEngine;

namespace Motk.Client.Combat.Units.Render
{
  public class CombatUnitFactory : ICombatUnitFactory
  {
    private readonly UnitVisualRegistry _unitVisualRegistry;
    
    public UniTask<CombatUnitEntity> CreateAsync(CombatUnitState state)
    {
      var visualDescriptor = _unitVisualRegistry.Require(state.DescriptorId);
      var view = Object.Instantiate(visualDescriptor.CombatPrefab);
      return UniTask.FromResult(new CombatUnitEntity(state, view));
    }

    public CombatUnitFactory(UnitVisualRegistry unitVisualRegistry) => _unitVisualRegistry = unitVisualRegistry;
  }
}