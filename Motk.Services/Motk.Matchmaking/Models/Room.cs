namespace Motk.Matchmaking.Models
{
  public record Room(string LocationId, int ServerId, int MaxUsers)
  {
    public HashSet<string> UserIds { get; } = new();
  }
}