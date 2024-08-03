using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Client.Core.Network.Client
{
  public readonly struct UnitSkillActionCommand
  {
    public readonly ushort SkillId;
    public readonly HexCoordinates Target;
  }
}