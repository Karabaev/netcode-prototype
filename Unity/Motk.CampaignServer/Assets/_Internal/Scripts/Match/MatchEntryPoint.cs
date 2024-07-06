using System;
using JetBrains.Annotations;
using Motk.CampaignServer.Match.States;
using UnityEngine;
using VContainer.Unity;

namespace Motk.CampaignServer.Match
{
  [UsedImplicitly]
  public class MatchEntryPoint : IStartable, IDisposable
  {
    private readonly MatchState _matchState;

    public MatchEntryPoint(MatchState matchState)
    {
      _matchState = matchState;
    }

    void IStartable.Start()
    {
      Debug.Log($"Match started. MatchId={_matchState.Id}");

      _matchState.Users.ItemAdded += State_OnUserAdded;
      _matchState.Users.ItemRemoved += State_OnUserRemoved;
    }

    void IDisposable.Dispose()
    {
      _matchState.Users.ItemAdded -= State_OnUserAdded;
      _matchState.Users.ItemRemoved -= State_OnUserRemoved;
      Debug.Log($"Match disposed. MatchId={_matchState.Id}");
    }
    
    private void State_OnUserAdded(string key, ushort newValue) => _matchState.ConnectedClients.Add(newValue);

    private void State_OnUserRemoved(string key, ushort oldValue) => _matchState.ConnectedClients.Remove(oldValue);
  }
}