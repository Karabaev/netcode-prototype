namespace Motk.Matchmaking
{
  public class Ticket
  {
    public string PlayerId { get; }

    public string Location { get; }

    public TicketStatus Status { get; set; }

    public Ticket(string playerId, string location)
    {
      PlayerId = playerId;
      Location = location;
      Status = TicketStatus.InProgress;
    }
  }
}