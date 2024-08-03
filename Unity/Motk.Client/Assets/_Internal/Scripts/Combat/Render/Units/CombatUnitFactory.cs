using Cysharp.Threading.Tasks;
using Motk.Combat.Client.Core.Units;
using Motk.Combat.Client.Core.Units.Services;
using Motk.Combat.Client.Render.Units.Descriptors;
using UnityEngine;

namespace Motk.Combat.Client.Render.Units
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