using System;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Matchmaking;
using Newtonsoft.Json;

namespace Motk.Client.Matchmaking
{
  [UsedImplicitly]
  public class MatchmakingClient : IDisposable
  {
    private const string BaseUrl = "http://localhost:5224";
    
    private readonly HttpClient _client;

    public MatchmakingClient() => _client = new HttpClient();

    public void Dispose() => _client.Dispose();

    public async UniTask<Guid> CreateTicketAsync(string playerId, string locationId)
    {
      using var content = new StringContent(string.Empty);
      var response = await _client.PostAsync($"{BaseUrl}/createTicket?userId={playerId}&locationId={locationId}", content);
      if (!response.IsSuccessStatusCode)
        throw new Exception("Failed to create ticket");
      
      var str = await response.Content.ReadAsStringAsync();
      return Guid.Parse(str.Replace("\"", ""));
    }

    public async UniTask<TicketStatusResponse> GetTicketStatusAsync(Guid ticketId)
    {
      using var content = new StringContent(string.Empty);
      var response = await _client.GetAsync($"{BaseUrl}/getTicketStatus?ticketId={ticketId}");

      if (!response.IsSuccessStatusCode)
        throw new Exception("Failed to obtain ticket status");
      
      var str = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<TicketStatusResponse>(str)!;
    }
  }
}