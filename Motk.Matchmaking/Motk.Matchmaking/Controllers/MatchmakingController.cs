using Microsoft.AspNetCore.Mvc;
using Motk.Matchmaking.Dtos;
using Motk.Matchmaking.Services;

namespace Motk.Matchmaking.Controllers;

[ApiController]
[Route("")]
public class MatchmakingController : ControllerBase
{
  private readonly MatchmakingService _matchmakingService;

  public MatchmakingController(MatchmakingService matchmakingService)
  {
    _matchmakingService = matchmakingService;
  }

  [HttpPost("[action]")]
  public Guid CreateTicket(string userId, string locationId)
  {
    return _matchmakingService.CreateTicket(userId, locationId);
  }

  [HttpGet("[action]")]
  public TicketStatusResponse GetTicketStatus(Guid ticketId)
  {
    return _matchmakingService.GetTicketStatus(ticketId);
  }

  [HttpGet("[action]")]
  public int GetRoomIdForUser(string userSecret)
  {
    return _matchmakingService.GetRoomIdForUser(userSecret);
  }

  [HttpGet("[action]")]
  public string GetLocationIdForRoom(int roomId)
  {
    return _matchmakingService.GetLocationIdForRoom(roomId);
  }

  [HttpPost("[action]")]
  public IActionResult RemoveUserFromRoom(string userSecret)
  {
    _matchmakingService.RemoveUserFromRoom(userSecret);
    return Ok();
  }
}