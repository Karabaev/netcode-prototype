using MagicOnion;
using MagicOnion.Server;

namespace Motk.Combat.Server.gRPC;

public interface IMyFirstService : IService<IMyFirstService>
{
  UnaryResult<int> SumAsync(int x, int y);
}

public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
{
  public async UnaryResult<int> SumAsync(int x, int y)
  {
    Console.WriteLine($"Received: {x}, {y}");
    return x + y;
  }
}