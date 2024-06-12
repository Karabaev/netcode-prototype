namespace Motk.Matchmaking
{
  public class TicketStatusResponse
  {
    public TicketStatus TicketStatus { get; }
    
    public ConnectionParameters? ConnectionParameters { get; }

    public TicketStatusResponse(TicketStatus ticketStatus, ConnectionParameters? connectionParameters)
    {
      TicketStatus = ticketStatus;
      ConnectionParameters = connectionParameters;
    }
  }
}