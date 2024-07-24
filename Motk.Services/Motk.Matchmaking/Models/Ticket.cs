namespace Motk.Matchmaking.Models
{
  public class Ticket
  {
    public string UserId { get; }

    public string LocationId { get; }

    public TicketStatus Status { get; set; }

    public Ticket(string userId, string locationId)
    {
      UserId = userId;
      LocationId = locationId;
      Status = TicketStatus.InProgress;
    }
  }
}