using Motk.CampaignServer.Locations;
using Motk.CampaignServer.Match.Net;
using Motk.CampaignServer.Match.States;
using Motk.CampaignServer.Server.Net;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.CampaignServer.Match
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
      
      builder.RegisterEntryPoint<MatchEntryPoint>();
      builder.RegisterEntryPoint<LocationController>();
      
      builder.Register<ActorMovementLogic>(Lifetime.Singleton);
      builder.Register<MatchMessageSender>(Lifetime.Singleton);
      builder.Register<MatchMessageReceiver>(Lifetime.Singleton);
    }
  }
}