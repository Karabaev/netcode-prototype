namespace Motk.Matchmaking
{
  public record TicketStatusResponse(string UserSecret, TicketStatus TicketStatus,
    ConnectionParameters? ConnectionParameters, int RoomId);
}