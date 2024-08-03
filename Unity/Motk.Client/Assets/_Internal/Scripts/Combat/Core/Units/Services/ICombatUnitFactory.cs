using Cysharp.Threading.Tasks;

namespace Motk.Combat.Client.Core.Units.Services
{
  public interface ICombatUnitFactory
  {
    UniTask<CombatUnitEntity> CreateAsync(CombatUnitState state);
  }
}