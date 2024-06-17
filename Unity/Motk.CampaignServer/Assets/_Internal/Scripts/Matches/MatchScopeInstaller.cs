using Motk.CampaignServer.Locations;
using Motk.CampaignServer.Matches.States;
using Motk.CampaignServer.Movement;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches
{
  public class MatchScopeInstaller : IInstaller
  {
    private readonly MatchState _matchState;
    private readonly Vector3 _locationOffset;

    public MatchScopeInstaller(MatchState state, Vector3 locationOffset)
    {
      _matchState = state;
      _locationOffset = locationOffset;
    }

    public void Install(IContainerBuilder builder)
    {
      builder.RegisterInstance(_matchState);
      builder.RegisterInstance(new LocationOffsetState(_locationOffset));
      builder.Register<CampaignLocationState>(Lifetime.Singleton);
      
      builder.RegisterEntryPoint<MatchLifeCycleController>();
      builder.RegisterEntryPoint<ConnectedPlayerLocationController>();
      builder.RegisterEntryPoint<LocationMovementController>();
      
      builder.Register<ActorMovementLogic>(Lifetime.Singleton);
    }
  }
}