using System.Threading.Tasks;
using MagicOnion;

namespace Motk.Combat.Shared.gRPC.Services
{
  public interface ICombatHub : IStreamingHub<ICombatHub, ICombatHubReceiver>
  {
    ValueTask<ushort> JoinAsync(string roomId, string userSecret);
    ValueTask LeaveAsync();
    ValueTask ReadyAsync();
    ValueTask UnitMoveActionAsync();
    ValueTask UnitWaitAction();
    ValueTask UnitDefendAction();
  }
}