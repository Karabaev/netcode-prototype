using JetBrains.Annotations;
using MagicOnion.Client;
using MessagePack;
using MessagePack.Resolvers;
using Motk.Combat.Shared.gRPC.Services;
using UnityEngine;

namespace Motk.Combat.Client.gRPC
{
  [UsedImplicitly]
  [MagicOnionClientGeneration(typeof(ICombatHubReceiver))]
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
}