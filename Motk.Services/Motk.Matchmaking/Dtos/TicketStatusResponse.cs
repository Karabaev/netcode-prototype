using Motk.Matchmaking.Models;

namespace Motk.Matchmaking.Dtos
{
  public record TicketStatusResponse(string UserSecret, TicketStatus TicketStatus,
    ConnectionParameters? ConnectionParameters, int RoomId);
}