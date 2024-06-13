using System.Collections.Generic;

namespace Motk.Matchmaking
{
  public record Room(string LocationId, int ServerId)
  {
    public HashSet<string> UserIds { get; } = new();
  }
}