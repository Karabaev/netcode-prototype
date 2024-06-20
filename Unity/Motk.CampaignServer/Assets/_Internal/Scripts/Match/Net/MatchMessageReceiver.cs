using System;
using JetBrains.Annotations;
using Motk.CampaignServer.Match.States;
using Motk.CampaignServer.Server.Net;
using Motk.Shared.Core.Net;

namespace Motk.CampaignServer.Match.Net
{
  [UsedImplicitly]
  public class MatchMessageReceiver
  {
    private readonly MatchState _matchState;
    private readonly ServerMessageReceiver _serverMessageReceiver;

    public MatchMessageReceiver(MatchState matchState, ServerMessageReceiver serverMessageReceiver)
    {
      _matchState = matchState;
      _serverMessageReceiver = serverMessageReceiver;
    }

    public void RegisterMessageHandler<T>(Action<ulong, T> action) where T : IMatchMessage, new()
    {
      _serverMessageReceiver.RegisterMatchMessageHandler(_matchState.Id, action);
    }

    public void UnregisterMessageHandler<T>() where T : IMatchMessage, new()
    {
      _serverMessageReceiver.UnregisterMatchMessageHandler<T>(_matchState.Id);
    }
  }
}