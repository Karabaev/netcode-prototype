using System.Collections.Generic;
using System.Linq;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using Motk.Client.Combat.Network.Server;
using Motk.Combat.Shared;
using Motk.HexGrid.Core.Descriptors;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Motk.Client.Combat.Network
{
  public class ServerMock
  {
    public UniTask<ushort> GetSelfTeamIdAsync()
    {
      return UniTask.FromResult((ushort) 1);
    }

    public UniTask<CombatStateMessage> GetCombatStateAsync()
    {
      var team1Units = new Dictionary<ushort, CombatUnitDto>
      {
        { 0, new CombatUnitDto("first", new HexCoordinates(2, -3), 1, 1) },
        { 1, new CombatUnitDto("first", new HexCoordinates(3, -3), 1, 1) },
        { 2, new CombatUnitDto("first", new HexCoordinates(4, -3), 1, 1) },
        { 3, new CombatUnitDto("first", new HexCoordinates(5, -3), 1, 1) },
        { 4, new CombatUnitDto("first", new HexCoordinates(6, -3), 1, 1) },
      };
      var team1 = new CombatTeamDto(team1Units);
      
      var team2Units = new Dictionary<ushort, CombatUnitDto>
      {
        { 0, new CombatUnitDto("second", new HexCoordinates(-3, 6), 1, 1) },
        { 1, new CombatUnitDto("second", new HexCoordinates(-2, 6), 1, 1) },
        { 2, new CombatUnitDto("second", new HexCoordinates(-1, 6), 1, 1) },
        { 3, new CombatUnitDto("second", new HexCoordinates(0, 6), 1, 1) },
        { 4, new CombatUnitDto("second", new HexCoordinates(1, 6), 1, 1) },
      };
      var team2 = new CombatTeamDto(team2Units);

      var teams = new Dictionary<ushort, CombatTeamDto>
      {
        { 0, team1 },
        { 1, team2 }
      };

      var team1UnitIds = team1Units
        .Select(u => new CombatUnitIdentifierDto(0, u.Key));
      var team2UnitIds = team2Units
        .Select(u => new CombatUnitIdentifierDto(1, u.Key));

      var turnsQueue = team1UnitIds.Union(team2UnitIds).ToArray();
      var random = new Random((uint) Time.frameCount);
      turnsQueue.Shuffle(ref random);
      var message = new CombatStateMessage(0, turnsQueue, teams);
      return UniTask.FromResult(message);
    }
  }
}