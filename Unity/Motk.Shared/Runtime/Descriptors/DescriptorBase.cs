using UnityEngine;

namespace Motk.Shared.Descriptors
{
  public abstract class DescriptorBase
  {
    [field: SerializeField]
    public string Id { get; private set; } = null!;
  }
}