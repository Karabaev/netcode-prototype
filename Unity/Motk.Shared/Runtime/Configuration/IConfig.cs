using Unity.Services.RemoteConfig;

namespace Motk.Shared.Configuration
{
  public interface IConfig
  {
    string MatchmakingServiceUrl { get; }
    
    string GameServerHost { get; }
    
    int GameServerPort { get; }
  }

  public class UnityRemoteConfig : IConfig
  {
    private const string MatchmakingUrlKey = "matchmakingUrl";
    private const string GameServerHostKey = "gameServerHost";
    private const string GameServerPortKey = "gameServerPort";

    public string MatchmakingServiceUrl => RemoteConfigService.Instance.appConfig.GetString(MatchmakingUrlKey);
    
    public string GameServerHost => RemoteConfigService.Instance.appConfig.GetString(GameServerHostKey);

    public int GameServerPort => RemoteConfigService.Instance.appConfig.GetInt(GameServerPortKey);
  }
}