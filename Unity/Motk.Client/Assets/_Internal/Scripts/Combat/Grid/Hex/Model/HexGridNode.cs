using Motk.Client.Combat.Grid.Abstract;
using Motk.Client.Combat.Grid.Hex.Descriptors;

namespace Motk.Client.Combat.Grid.Hex.Model
{
  public class HexGridNode : GraphNode<HexCoordinates, CombatGridPayload>
  {
    public readonly HexCoordinates Coordinates;
    
    public bool IsWalkable => Payload.IsWalkable;

    public HexGridNode(HexCoordinates id, CombatGridPayload payload) : base(id, payload)
    {
      Coordinates = id;
    }
  }
}