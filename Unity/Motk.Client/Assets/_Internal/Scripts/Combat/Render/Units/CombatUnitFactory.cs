using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity.Functions;
using Motk.Combat.Client.Core.Units;
using Motk.Combat.Client.Core.Units.Services;
using Motk.Combat.Client.Render.Units.Descriptors;
using UnityEngine;

namespace Motk.Combat.Client.Render.Units
{
  [UsedImplicitly]
  public class CombatUnitFactory : ICombatUnitFactory
  {
    private readonly UnitVisualRegistry _unitVisualRegistry;
    private readonly IHexGridFunctions _hexGridFunctions;
    
    public UniTask<CombatUnitEntity> CreateAsync(CombatUnitState state)
    {
      var visualDescriptor = _unitVisualRegistry.Require(state.DescriptorId);
      var view = Object.Instantiate(visualDescriptor.CombatPrefab);
      return UniTask.FromResult(new CombatUnitEntity(state, view, _hexGridFunctions));
    }

    public CombatUnitFactory(UnitVisualRegistry unitVisualRegistry, IHexGridFunctions hexGridFunctions)
    {
      _unitVisualRegistry = unitVisualRegistry;
      _hexGridFunctions = hexGridFunctions;
    }
  }
}