using System.Text;
using com.karabaev.utilities.unity;
using com.karabaev.utilities.unity.GameKit;
using Motk.CampaignServer.Match.States;
using Motk.CampaignServer.Server.States;
using TMPro;
using UnityEngine;

namespace Motk.CampaignServer.DebugSystem
{
  public class ServerDebugUIView : GameKitComponent
  {
    [SerializeField, HideInInspector, RequireInChild("PlayersCountText")]
    private TMP_Text _playersCountText = null!;
    [SerializeField, HideInInspector, RequireInChild("MatchesInfoText")]
    private TMP_Text _matchesInfoText = null!;

    private ServerState _serverState = null!;

    public void Construct(ServerState serverState)
    {
      _serverState = serverState;
      
      _serverState.Matches.ItemAdded += State_OnMatchAdded;
      _serverState.Matches.ItemRemoved += State_OnMatchRemoved;

      UpdateDebugInfo();
    }

    private void OnDestroy()
    {
      if (_serverState == null!)
        return;
      
      _serverState.Matches.ItemAdded -= State_OnMatchAdded;
      _serverState.Matches.ItemRemoved -= State_OnMatchRemoved;
    }

    private void State_OnMatchAdded(int matchId, MatchState match)
    {
      match.Users.ItemAdded += State_OnUserAddedToMatch;
      match.Users.ItemAdded += State_OnUserRemovedFromMatch;
    }

    private void State_OnMatchRemoved(int matchId, MatchState match)
    {
      match.Users.ItemAdded -= State_OnUserAddedToMatch;
      match.Users.ItemAdded -= State_OnUserRemovedFromMatch;
    }

    private void State_OnUserAddedToMatch(string userSecret, ushort clientId) => UpdateDebugInfo();

    private void State_OnUserRemovedFromMatch(string userSecret, ushort clientId) => UpdateDebugInfo();

    private void UpdateDebugInfo()
    {
      _playersCountText.text = $"TotalPlayers={_serverState.ClientsInMatches.Count}";

      if (_serverState.Matches.Count == 0)
      {
        _matchesInfoText.text = "There is no matches";
        return;
      }
      
      var sb = new StringBuilder();
      foreach (var (matchId, match) in _serverState.Matches)
        sb.AppendLine($"Match_{matchId}={match.Users.Count}");
      
      _matchesInfoText.text = sb.ToString();
    }
  }
}