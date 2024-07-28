using System;
using System.Collections.Generic;

namespace Motk.HexGrid.Core.Descriptors
{
  [Serializable]
  public class HexMapDescriptor
  {
    public IReadOnlyList<HexMapNodeDescriptor> Nodes { get; }

    public HexMapDescriptor(IReadOnlyList<HexMapNodeDescriptor> nodes) => Nodes = nodes;
  }
}