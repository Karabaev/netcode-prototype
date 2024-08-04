using MagicOnion;
using MagicOnion.Server;
using Motk.Combat.Shared.gRPC;

namespace Motk.Combat.Server.gRPC;

public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
{
  public async UnaryResult<int> SumAsync(int x, int y)
  {
    Console.WriteLine($"Received: {x}, {y}");
    return x + y;
  }
}