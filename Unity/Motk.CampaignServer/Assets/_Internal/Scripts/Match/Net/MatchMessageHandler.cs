using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Motk.CampaignServer.Server;
using Motk.CampaignServer.Server.Net;
using VContainer.Unity;

namespace Motk.CampaignServer.Match.Net
{
  [UsedImplicitly]
  public class MatchMessageHandler : IStartable, IDisposable, IMatchMessageReceiver, IMatchMessageHandler
  {
    private readonly MatchState _matchState;
    private readonly ServerMessageReceiver _messageReceiver;

    private readonly Dictionary<Type, Delegate> _messageHandlers = new();

    public MatchMessageHandler(MatchState matchState, ServerMessageReceiver messageReceiver)
    {
      _matchState = matchState;
      _messageReceiver = messageReceiver;
    }

    void IStartable.Start() => _messageReceiver.AddMatchHandler(_matchState.Id, this);

    void IDisposable.Dispose() => _messageReceiver.RemoveMatchHandler(_matchState.Id);

    void IMatchMessageHandler.RegisterMessageHandler<T>(Action<ulong, T> handler) => _messageHandlers.Add(typeof(T), handler);

    void IMatchMessageHandler.UnregisterMessageHandler<T>() => _messageHandlers.Remove(typeof(T));

    void IMatchMessageReceiver.OnMessageReceived<T>(ulong clientId, T message)
    {
      if (!_messageHandlers.TryGetValue(typeof(T), out var handlers))
        return;

      foreach (var handler in handlers.GetInvocationList())
        ((Action<ulong, T>)handler).Invoke(clientId, message);
    }
  }
}