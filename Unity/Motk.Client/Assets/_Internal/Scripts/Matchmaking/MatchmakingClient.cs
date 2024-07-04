using System;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Matchmaking;
using Motk.Shared.Configuration;
using Newtonsoft.Json;

namespace Motk.Client.Matchmaking
{
  [UsedImplicitly]
  public class MatchmakingClient : IDisposable
  {
    private readonly HttpClient _client;

    public MatchmakingClient(IConfig config)
    {
      _client = new HttpClient();
      _client.BaseAddress = new Uri(config.MatchmakingServiceUrl);
    }

    public void Dispose() => _client.Dispose();

    public async UniTask<Guid> CreateTicketAsync(string playerId, string locationId)
    {
      using var content = new StringContent(string.Empty);
      var response = await _client.PostAsync($"createTicket?userId={playerId}&locationId={locationId}", content);
      if (!response.IsSuccessStatusCode)
        throw new Exception("Failed to create ticket");
      
      var str = await response.Content.ReadAsStringAsync();
      return Guid.Parse(str.Replace("\"", ""));
    }

    public async UniTask<TicketStatusResponse> GetTicketStatusAsync(Guid ticketId)
    {
      using var content = new StringContent(string.Empty);
      var response = await _client.GetAsync($"getTicketStatus?ticketId={ticketId}");

      if (!response.IsSuccessStatusCode)
        throw new Exception("Failed to obtain ticket status");
      
      var str = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<TicketStatusResponse>(str)!;
    }
  }
}