using Motk.CampaignServer.Locations;
using Motk.CampaignServer.Matches.States;
using Motk.CampaignServer.Movement;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Locations;
using VContainer;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches
{
  public static class MatchScopeInstaller
  {
    public static void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<CampaignLocationState>(Lifetime.Singleton);
      builder.Register<LocationOffsetState>(Lifetime.Singleton);
      builder.Register<MatchState>(Lifetime.Singleton);
      builder.RegisterEntryPoint<MatchLifeCycleController>();
      builder.RegisterEntryPoint<ConnectedPlayerController>();
      builder.RegisterEntryPoint<LocationMovementController>();
      builder.Register<ActorMovementLogic>(Lifetime.Singleton);
    }
  }
}