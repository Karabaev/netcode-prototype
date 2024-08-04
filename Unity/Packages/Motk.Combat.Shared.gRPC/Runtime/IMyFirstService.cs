using MagicOnion;

namespace Motk.Combat.Shared.gRPC
{
  public interface IMyFirstService : IService<IMyFirstService>
  {
    UnaryResult<int> SumAsync(int x, int y);
  }
}