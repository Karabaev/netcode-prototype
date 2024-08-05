using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Motk.Descriptors.FileSystem
{
  public class DescriptorLoaderFromFileSystem : IDescriptorLoader, IDisposable
  {
    private readonly IFileSystemOperations _fileSystemOperations;
    private readonly IDescriptorsRootDirectoryProvider _rootDirectoryProvider;
    /// <summary>
    /// Category to type to descriptor data.
    /// </summary>
    private Dictionary<string, Dictionary<string, List<DescriptorData>>> _descriptors = new();

    public Task LoadAsync(CancellationToken ct)
    {
      var rootDirectory = _rootDirectoryProvider.GetRootDirectory();
      var descriptorFiles = _fileSystemOperations
        .EnumerateFiles(rootDirectory, "*.descriptor", SearchOption.AllDirectories);

      var tasks = new List<Task>();
      foreach (var file in descriptorFiles)
      {
        var directory = _fileSystemOperations.GetParentFullName(file)!;
        var category = Path.GetFileName(_fileSystemOperations.GetParentFullName(directory))!;
      
        if (!_descriptors.TryGetValue(category, out var categoryDescriptors))
        {
          categoryDescriptors = new Dictionary<string, List<DescriptorData>>();
          _descriptors[category] = categoryDescriptors;
        }
      
        var descriptorType = Path.GetFileNameWithoutExtension(file)!;
        if (!categoryDescriptors.TryGetValue(descriptorType, out var typedDescriptors))
        {
          typedDescriptors = new List<DescriptorData>();
          categoryDescriptors[descriptorType] = typedDescriptors;
        }
        
        // todokmo possible multiple write operations
        var task = LoadDescriptorDataAsync(file, ct)
          .ContinueWith(data => typedDescriptors.Add(data.Result), ct);
        tasks.Add(task);
      }

      return Task.WhenAll(tasks);
    }

    public Task<IReadOnlyList<DescriptorData>> GetDescriptorsAsync(string category, string type, CancellationToken ct)
    {
      return Task.FromResult<IReadOnlyList<DescriptorData>>(_descriptors[category][type]);
    }
    
    private async Task<DescriptorData> LoadDescriptorDataAsync(string path, CancellationToken ct)
    {
      var fileData = await _fileSystemOperations.ReadAllBytesAsync(path, ct);
      var descriptorId = Path.GetFileName(Path.GetDirectoryName(path))!;
      return new DescriptorData(descriptorId, fileData);
    }

    public void Dispose()
    {
      _descriptors = null!;
    }

    public DescriptorLoaderFromFileSystem(IFileSystemOperations fileSystemOperations,
      IDescriptorsRootDirectoryProvider rootDirectoryProvider)
    {
      _fileSystemOperations = fileSystemOperations;
      _rootDirectoryProvider = rootDirectoryProvider;
    }
  }
}