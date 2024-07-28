using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Motk.Shared.Descriptors
{
  public class DescriptorRegistry<T> : ScriptableObject where T : DescriptorBase
  {
    [SerializeField]
    private List<T> _entries = null!;

    public IReadOnlyDictionary<string, T> Entries => _entries.ToDictionary(l => l.Id, l => l);
  }
}