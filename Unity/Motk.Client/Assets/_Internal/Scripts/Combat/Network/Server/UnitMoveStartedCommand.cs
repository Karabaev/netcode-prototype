using Motk.Combat.Shared;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Client.Combat.Network.Server
{
  public readonly struct UnitMoveStartedCommand
  {
    public readonly CombatUnitIdentifierDto UnitId;
    public readonly HexCoordinates[] Path;

    public UnitMoveStartedCommand(CombatUnitIdentifierDto unitId, HexCoordinates[] path)
    {
      UnitId = unitId;
      Path = path;
    }
  }
}