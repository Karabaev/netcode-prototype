using Motk.Descriptors.Registry;
using VContainer;

namespace Motk.Client.Core.Descriptors
{
  public static class DescriptorDIUtils
  {
    public static void RegisterDescriptorRegistry<T>(this IContainerBuilder builder) where T : IMutableDescriptorRegistry
    {
      builder.Register<T>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
    }
  }
}