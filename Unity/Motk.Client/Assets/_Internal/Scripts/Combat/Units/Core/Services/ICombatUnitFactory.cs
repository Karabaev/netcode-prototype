using Cysharp.Threading.Tasks;

namespace Motk.Client.Combat.Units.Core.Services
{
  public interface ICombatUnitFactory
  {
    UniTask<CombatUnitEntity> CreateAsync(CombatUnitState state);
  }
}