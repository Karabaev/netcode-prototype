namespace Motk.Matchmaking.Models
{
  public record Room(string LocationId, int ServerId)
  {
    public HashSet<string> UserIds { get; } = new();
  }
}