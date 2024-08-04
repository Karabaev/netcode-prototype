using MagicOnion.Server.Hubs;
using Motk.Combat.Server.gRPC.Utils;
using Motk.Combat.Shared.gRPC.Services;
using Motk.Combat.Shared.Messages.Dto;
using Motk.HexGrid.Core;

namespace Motk.Combat.Server.gRPC;

public class CombatHub : StreamingHubBase<ICombatHub, ICombatHubReceiver>, ICombatHub
{
  private readonly ILogger<CombatHub> _logger;
  private readonly MetaStoreStub _metaStoreStub;
  private readonly InitialUnitPlacementProvider _placementProvider;
  
  private IGroup _room = null!;
  private IInMemoryStorage<CombatPlayer> _playersStorage = null!;

  public async ValueTask<ushort> JoinAsync(string roomId, string userSecret)
  {
    _room = Group.RawGroupRepository.GetOrAdd(roomId);
    _playersStorage = _room.GetInMemoryStorage<CombatPlayer>();

    byte teamId = 0;
    foreach (var player in _playersStorage.AllValues)
      teamId = Math.Max(teamId, player.TeamId);

    var newPlayer = new CombatPlayer(userSecret, teamId);
    (_room, _playersStorage) = await Group.AddAsync(roomId, newPlayer);

    // request army info from meta service
    var army = await _metaStoreStub.GetHeroArmyAsync(userSecret);

    var combatUnits = army
      .Select(u =>
      {
        // _placementProvider.GetUnitPlacement()
        return new CombatUnitDto(u.DescriptorId, u.Id, new HexCoordinatesDto(), HexDirection.E, u.Count, 100);
      })
      .ToArray();
    
    var teamDto = new CombatTeamDto(newPlayer.TeamId, combatUnits);
    
    Broadcast(_room).OnTeamJoined(teamDto);
    _logger.LogInformation($"Player joined. RoomId={roomId}, UserSecret={userSecret}");
    return teamId;
  }

  public async ValueTask LeaveAsync()
  {
    var player = _playersStorage.Require(Context.ContextId);
    await _room.RemoveAsync(Context);

    Broadcast(_room).OnTeamLeft(player.TeamId);
    _logger.LogInformation($"Player left. UserSecret={player.UserSecret}");
  }

  public ValueTask ReadyAsync()
  {
    var player = _playersStorage.Require(Context.ContextId);
    player.IsReady = true;
    
    foreach (var allPlayers in _playersStorage.AllValues)
    {
      if (!allPlayers.IsReady)
        return ValueTask.CompletedTask;
    }
    
    Broadcast(_room).OnCombatStarted();
    _logger.LogInformation($"Combat started. CombatId={_room.GroupName}");
    return ValueTask.CompletedTask;
  }

  public ValueTask UnitMoveActionAsync() => ValueTask.CompletedTask;

  public ValueTask UnitWaitAction() => ValueTask.CompletedTask;

  public ValueTask UnitDefendAction() => ValueTask.CompletedTask;

  protected override ValueTask OnDisconnected() => LeaveAsync();
  
  public CombatHub(ILogger<CombatHub> logger, MetaStoreStub metaStoreStub, InitialUnitPlacementProvider placementProvider)
  {
    _logger = logger;
    _metaStoreStub = metaStoreStub;
    _placementProvider = placementProvider;
  }
}