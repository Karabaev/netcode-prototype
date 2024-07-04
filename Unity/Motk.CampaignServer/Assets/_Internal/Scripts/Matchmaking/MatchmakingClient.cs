using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Shared.Configuration;

namespace Motk.CampaignServer.Matchmaking
{
  [UsedImplicitly]
  public class MatchmakingClient : IDisposable
  {
    private readonly HttpClient _httpClient;

    public MatchmakingClient(IConfig config)
    {
      _httpClient = new HttpClient();
      _httpClient.BaseAddress = new Uri(config.MatchmakingServiceUrl);
    }

    public void Dispose() => _httpClient.Dispose();

    public async Task<int> GetRoomIdForUserAsync(string userSecret)
    {
      using var content = new StringContent(string.Empty);
      var response = await _httpClient.GetAsync($"getRoomIdForUser?userSecret={userSecret}");

      if (!response.IsSuccessStatusCode)
        throw new Exception("Failed to get room id for user");
      
      var str = await response.Content.ReadAsStringAsync();
      return int.Parse(str.Replace("\"", string.Empty));
    }

    public async Task<string> GetLocationIdForRoomAsync(int matchId)
    {
      using var content = new StringContent(string.Empty);
      var response = await _httpClient.GetAsync($"getLocationIdForRoom?roomId={matchId}");

      if (!response.IsSuccessStatusCode)
        throw new Exception("Failed to get location id for room");
      
      var str = await response.Content.ReadAsStringAsync();
      return str.Replace("\"", string.Empty);
    }
    
    public async UniTask RemoveUserFromRoomAsync(string userSecret)
    {
      using var content = new StringContent(string.Empty);
      var response = await _httpClient.PostAsync($"removeUserFromRoom?userSecret={userSecret}", content);
      if (!response.IsSuccessStatusCode)
        throw new Exception("Failed remove user from room");
    }
  }
}