using com.karabaev.reactivetypes.Dictionary;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches.States
{
  [UsedImplicitly]
  public class MatchState
  {
    public int Id { get; }
    
    public string LocationId { get; }
    
    // todokmo точно ли нужен скоуп? Можно его резолвить отдельно?
    public LifetimeScope Scope { get; set; }

    /// <summary>
    /// Ключ - секрет пользвателя, значение - id соединения.
    /// </summary>
    public ReactiveDictionary<string, ulong> Users { get; }

    public MatchState(int id, string locationId)
    {
      Id = id;
      LocationId = locationId;
      Users = new();
    }
  }
}