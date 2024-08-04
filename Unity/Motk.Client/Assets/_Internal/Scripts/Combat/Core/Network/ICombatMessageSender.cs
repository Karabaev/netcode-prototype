using Cysharp.Threading.Tasks;

namespace Motk.Combat.Client.Core.Network
{
  public interface ICombatMessageSender
  {
    UniTask ConnectAsync();
    UniTask WaitForDisconnect();
    UniTask<ushort> JoinRoomAsync(string roomId, string userSecret);
    UniTask LeaveRoomAsync();
    UniTask ReadyAsync();
    UniTask MakeUnitMoveActionAsync();
  }
}