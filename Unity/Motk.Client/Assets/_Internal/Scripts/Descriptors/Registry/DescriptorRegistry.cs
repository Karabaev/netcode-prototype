using System;
using System.Collections.Generic;

namespace Motk.Descriptors.Registry
{
  public abstract class DescriptorRegistry<TId, TEntry> : IMutableDescriptorRegistry
    where TId : IEquatable<TId>
  {
    private readonly Dictionary<TId, TEntry> _entries = new();

    public bool TryGet(TId id, out TEntry? entry)
    {
      return _entries.TryGetValue(id, out entry);
    }

    public TEntry? Get(TId id)
    {
      TryGet(id, out var result);
      return result;
    }

    public TEntry Require(TId id)
    {
      if (!TryGet(id, out var result))
        throw new KeyNotFoundException($"Entry not found. Id={id}, Registry={GetType().Name}");

      return result!;
    }

    public bool Contains(TId id) => _entries.ContainsKey(id);

    Type IMutableDescriptorRegistry.EntryType => typeof(TEntry);
    
    string IMutableDescriptorRegistry.Type => GetType().Name.Replace("Registry", string.Empty);

    string IMutableDescriptorRegistry.Category => Category;
    protected abstract string Category { get; }
    
    void IMutableDescriptorRegistry.Add(object id, object entry) => _entries.Add((TId)id, (TEntry)entry);

    void IMutableDescriptorRegistry.Remove(object id) => _entries.Remove((TId)id);
    
    public Dictionary<TId, TEntry>.Enumerator GetEnumerator() => _entries.GetEnumerator();
  }
}