using System;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Motk.Client
{
  [UsedImplicitly]
  public class ConnectionAppState : ApplicationState<ConnectionAppState.Context>
  {
    private readonly NetworkManager _networkManager;
    private readonly AppScopeState _appScopeState;

    public override async UniTask EnterAsync(Context context)
    {
      Object.FindObjectOfType<UnityTransport>().OnTransportEvent += OnTransportEvent;

      await UniTask.Yield();
      
      _networkManager.StartClient();
      _networkManager.OnConnectionEvent += NetworkManagerOnOnConnectionEvent;
    }

    private void NetworkManagerOnOnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2)
    {
      Debug.Log($"{arg2.EventType}");
      
      if (arg2.EventType == ConnectionEvent.ClientConnected)
      {
        EnterNextStateAsync<CampaignAppState, CampaignAppState.Context>(new CampaignAppState.Context(_appScopeState.AppScope))
          .Forget();
      }

    }

    private void OnTransportEvent(NetworkEvent eventType, ulong clientId, ArraySegment<byte> payload, float receiveTime)
    {

    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    public ConnectionAppState(ApplicationStateMachine stateMachine, NetworkManager networkManager, AppScopeState appScopeState) : base(stateMachine)
    {
      _networkManager = networkManager;
      _appScopeState = appScopeState;
    }

    public record Context(LifetimeScope ParentScope);
  }
}