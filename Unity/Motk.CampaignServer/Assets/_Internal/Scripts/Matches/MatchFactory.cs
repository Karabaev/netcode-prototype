using JetBrains.Annotations;
using Motk.CampaignServer.Matches.States;
using UnityEngine;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches
{
  [UsedImplicitly]
  public class MatchFactory
  {
    public MatchState Create(int id, string locationId, Vector3 locationOffset, LifetimeScope parentScope)
    {
      var matchState = new MatchState(id, locationId);      
      var scopeInstaller = new MatchScopeInstaller(matchState, locationOffset);
      var matchScope = parentScope.CreateChild(scopeInstaller);
      matchState.Scope = matchScope;
      return matchState;
    }
  }
}