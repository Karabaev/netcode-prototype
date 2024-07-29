using Motk.HexGrid.Core.Descriptors;

namespace Motk.Client.Combat.Network.Client
{
  public readonly struct UnitSkillActionCommand
  {
    public readonly ushort SkillId;
    public readonly HexCoordinates Target;
  }
}