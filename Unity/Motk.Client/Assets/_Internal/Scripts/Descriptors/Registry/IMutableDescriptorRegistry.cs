using System;

namespace Motk.Descriptors.Registry
{
  public interface IMutableDescriptorRegistry
  {
    Type EntryType { get; }
    string Type { get; }
    string Category { get; }
    
    void Add(object id, object descriptor);
    void Remove(object id);
  }
}