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
  public Task<Guid> CreateTicket(string userId, string locationId)
  {
    return _matchmakingService.CreateTicketAsync(userId, locationId);
  }

  [HttpGet("[action]")]
  public Task<TicketStatusResponse> GetTicketStatus(Guid ticketId)
  {
    return _matchmakingService.GetTicketStatusAsync(ticketId);
  }

  [HttpGet("[action]")]
  public Task<int> GetRoomIdForUser(string userSecret)
  {
    return _matchmakingService.GetRoomIdForUserAsync(userSecret);
  }

  [HttpGet("[action]")]
  public Task<string> GetLocationIdForRoom(int roomId)
  {
    return _matchmakingService.GetLocationIdForRoomAsync(roomId);
  }

  [HttpPost("[action]")]
  public Task RemoveUserFromRoom(string userSecret)
  {
    return _matchmakingService.RemoveUserFromRoomAsync(userSecret);
  }
}