using System;
using Motk.Shared.Core.Net;

namespace Motk.CampaignServer.Match.Net
{
  public interface IMatchMessageHandler 
  {
    void RegisterMessageHandler<T>(Action<ulong, T> handler) where T : IMatchMessage, new();
    
    void UnregisterMessageHandler<T>() where T : IMatchMessage, new();
  }
}