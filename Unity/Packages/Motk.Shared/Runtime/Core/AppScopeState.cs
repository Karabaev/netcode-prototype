using JetBrains.Annotations;
using VContainer.Unity;

namespace Motk.Shared.Core
{
  [UsedImplicitly]
  public class AppScopeState
  {
    public LifetimeScope AppScope { get; set; } = null!;
  }
}