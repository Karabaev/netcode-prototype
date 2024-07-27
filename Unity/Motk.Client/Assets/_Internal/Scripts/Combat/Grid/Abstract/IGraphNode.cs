using System.Collections.Generic;

namespace Motk.Client.Combat.Grid.Abstract
{
  public interface IGraphNode : IMutableGraphNode
  {
    IReadOnlyCollection<IGraphNode> Neighbors { get; }
  }
  
  public interface IMutableGraphNode
  {
    void AddNeighbor(IMutableGraphNode node);

    void RemoveNeighbor(IMutableGraphNode node);
  }
}