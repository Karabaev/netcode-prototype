using JetBrains.Annotations;
using Motk.CampaignServer.Match.States;
using UnityEngine;
using VContainer.Unity;

namespace Motk.CampaignServer.Match
{
  [UsedImplicitly]
  public class MatchFactory
  {
    public MatchState Create(int id, string locationId, Vector3 locationOffset, LifetimeScope parentScope)
    {
      var matchState = new MatchState(id, locationId);      
      var scopeInstaller = new MatchScopeInstaller(matchState, locationOffset);
      var matchScope = parentScope.CreateChild(scopeInstaller);
      matchScope.name = $"Match{id}_{locationId}";
      matchState.Scope = matchScope;
      return matchState;
    }
  }
}