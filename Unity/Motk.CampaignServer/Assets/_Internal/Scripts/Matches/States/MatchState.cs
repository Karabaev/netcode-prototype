using com.karabaev.reactivetypes.Dictionary;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches.States
{
  [UsedImplicitly]
  public class MatchState
  {
    public string LocationId { get; set; } = null!;
    
    // todokmo точно ли нужен скоуп? Можно его резолвить отдельно?
    public LifetimeScope Scope { get; set; } = null!;
    
    /// <summary>
    /// Ключ - секрет пользвателя, значение - id соединения.
    /// </summary>
    public ReactiveDictionary<string, ulong> Users { get; } = new();
  }
}