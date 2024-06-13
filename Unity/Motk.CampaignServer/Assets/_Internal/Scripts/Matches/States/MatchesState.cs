using com.karabaev.reactivetypes.Dictionary;
using JetBrains.Annotations;

namespace Motk.CampaignServer.Matches.States
{
  [UsedImplicitly]
  public class MatchesState
  {
    /// <summary>
    /// Ключ - id комнаты в матчмейкинге.
    /// </summary>
    public ReactiveDictionary<int, MatchState> Matches { get; } = new();
  }
}