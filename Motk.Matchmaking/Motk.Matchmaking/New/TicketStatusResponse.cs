namespace Motk.Matchmaking.New
{
  public record TicketStatusResponse(string UserSecret, TicketStatus TicketStatus,
    ConnectionParameters? ConnectionParameters, int RoomId);
}