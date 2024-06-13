using System.Collections.Generic;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches
{
  public class MatchState
  {
    public LifetimeScope Scope { get; }
    
    public HashSet<int> UserIds { get; }

    public MatchState(LifetimeScope scope)
    {
      Scope = scope;
      UserIds = new HashSet<int>();
    }
  }
}