using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Motk.Descriptors.Registry;
using Motk.Descriptors.Serialization;

namespace Motk.Descriptors
{
  public class DescriptorsBootstrapper
  {
    private readonly IDescriptorLoader _loader;
    private readonly IReadOnlyList<IMutableDescriptorRegistry> _registries;
    private readonly IDescriptorSerializer _serializer;

    public async ValueTask BootstrapAsync(CancellationToken ct)
    {
      await _loader.LoadAsync(ct);
      
      if (ct.IsCancellationRequested)
        return;
      
      await Task.WhenAll(_registries.Select(r => PopulateRegistryAsync(r, ct)));
    }

    private async Task PopulateRegistryAsync(IMutableDescriptorRegistry registry, CancellationToken ct)
    {
      var descriptors = await _loader.GetDescriptorsAsync(registry.Category, registry.Type, ct);
      
      foreach (var descriptorData in descriptors)
      {
        try
        {
          var entry = (await _serializer.DeserializeAsync(descriptorData.Data, registry.EntryType))!;
          registry.Add(descriptorData.Id, entry);
        }
        catch (Exception e)
        {
          throw new Exception($"Deserialize failed. DescriptorId={descriptorData.Id}", e);
        }
      }
    }

    public DescriptorsBootstrapper(IDescriptorLoader loader, IReadOnlyList<IMutableDescriptorRegistry> registries,
      IDescriptorSerializer serializer)
    {
      _loader = loader;
      _registries = registries;
      _serializer = serializer;
    }
  }
}