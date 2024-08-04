using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MagicOnion.Client;
using Motk.Client.Core;
using Motk.Combat.Client.Core.Network;
using Motk.Combat.Shared.gRPC.Services;
using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Client.gRPC
{
  [UsedImplicitly]
  public class CombatHubClient : ICombatHubReceiver, ICombatMessageReceiver, ICombatMessageSender, IDisposable
  {
    public event NetworkMessageHandler<CombatTeamDto>? TeamJoined;
    public event NetworkMessageHandler<ushort>? TeamLeft;
    public event NetworkMessageHandler? UnitMoveAction;
    public event NetworkMessageHandler? UnitWaitAction;
    public event NetworkMessageHandler? UnitDefendAction;
    public event NetworkMessageHandler? CombatStarted;

    private readonly GrpcChannelState _grpcChannelState; 
    private ICombatHub _combatHubClient = null!;

    async UniTask ICombatMessageSender.ConnectAsync()
    {
      _combatHubClient = await StreamingHubClient
        .ConnectAsync<ICombatHub, ICombatHubReceiver>(_grpcChannelState.GrpcChannel, this);
    }

    void IDisposable.Dispose() => _combatHubClient.DisposeAsync().AsUniTask().Forget();

    UniTask ICombatMessageSender.WaitForDisconnect() => _combatHubClient.WaitForDisconnect().AsUniTask();

    UniTask<ushort> ICombatMessageSender.JoinRoomAsync(string roomId, string userSecret)
    {
      return _combatHubClient.JoinAsync(roomId, userSecret).AsUniTask();
    }

    UniTask ICombatMessageSender.LeaveRoomAsync()
    {
      return _combatHubClient.LeaveAsync().AsUniTask();
    }

    UniTask ICombatMessageSender.ReadyAsync()
    {
      return _combatHubClient.ReadyAsync().AsUniTask();
    }

    UniTask ICombatMessageSender.MakeUnitMoveActionAsync()
    {
      return _combatHubClient.UnitMoveActionAsync().AsUniTask();
    }

    void ICombatHubReceiver.OnTeamJoined(CombatTeamDto team) => TeamJoined?.Invoke(in team);

    void ICombatHubReceiver.OnTeamLeft(ushort teamId) => TeamLeft?.Invoke(in teamId);

    void ICombatHubReceiver.OnUnitMoveAction() => UnitMoveAction?.Invoke();

    void ICombatHubReceiver.OnUnitWaitAction() => UnitWaitAction?.Invoke();

    void ICombatHubReceiver.OnUnitDefendAction() => UnitDefendAction?.Invoke();
    
    void ICombatHubReceiver.OnCombatStarted() => CombatStarted?.Invoke();

    public CombatHubClient(GrpcChannelState grpcChannelState) => _grpcChannelState = grpcChannelState;
  }
}