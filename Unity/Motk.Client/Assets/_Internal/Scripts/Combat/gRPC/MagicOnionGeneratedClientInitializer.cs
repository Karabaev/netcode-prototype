using JetBrains.Annotations;
using MagicOnion;
using MagicOnion.Client;
using MessagePack;
using MessagePack.Resolvers;
using Motk.Combat.Client.gRPC.Motk.Combat.Server.gRPC;
using UnityEngine;

namespace Motk.Combat.Client.gRPC
{
  [UsedImplicitly]
  [MagicOnionClientGeneration(typeof(IMyFirstService))]
  public partial class MagicOnionGeneratedClientInitializer { }

  public static class RegisterResolvers
  {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Register()
    {
      StaticCompositeResolver.Instance.Register(
        MagicOnionGeneratedClientInitializer.Resolver,
        GeneratedResolver.Instance,
        BuiltinResolver.Instance,
        PrimitiveObjectResolver.Instance);

      var options = MessagePackSerializer.DefaultOptions.WithResolver(StaticCompositeResolver.Instance);
      MessagePackSerializer.DefaultOptions = options;
    }
  }
  
  namespace Motk.Combat.Server.gRPC
  {
    public interface IMyFirstService : IService<IMyFirstService>
    {
      UnaryResult<int> SumAsync(int x, int y);
    }
  }
}