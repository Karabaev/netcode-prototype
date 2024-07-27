using System.Collections.Generic;

namespace Motk.Client.Combat.Grid.Hex.Descriptors
{
  public class HexMapDescriptor
  {
    public IReadOnlyList<HexMapNodeDescriptor> Nodes { get; set; } = null!;
  }
}