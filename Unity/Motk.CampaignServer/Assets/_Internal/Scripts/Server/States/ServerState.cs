using System.Collections.Generic;
using com.karabaev.reactivetypes.Dictionary;
using JetBrains.Annotations;
using Motk.CampaignServer.Match.States;

namespace Motk.CampaignServer.Server.States
{
  [UsedImplicitly]
  public class ServerState
  {
    public HashSet<int> PreparingMatches { get; } = new();

    public ReactiveDictionary<int, MatchState> Matches { get; } = new();

    public Dictionary<ulong, int> ClientsInMatches { get; } = new();
  }
}