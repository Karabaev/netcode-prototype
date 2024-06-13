using System;
using Motk.CampaignServer.Matches.States;
using Motk.Shared.Locations;
using VContainer;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches
{
  public static class MatchScopeInstaller
  {
    public static void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<MatchState>(Lifetime.Singleton);
      builder.Register<CampaignLocationState>(Lifetime.Singleton);
      builder.Register<MatchLifeCycleController>(Lifetime.Singleton).As<IStartable>().As<IDisposable>();
    }
  }
}