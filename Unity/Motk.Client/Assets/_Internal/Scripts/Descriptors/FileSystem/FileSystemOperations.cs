using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Motk.Descriptors.FileSystem
{
  public class FileSystemOperations : IFileSystemOperations
  {
    public IEnumerable<string> EnumerateFiles(string root, string pattern, SearchOption searchOption)
    {
      return Directory.EnumerateFiles(root, pattern, searchOption);
    }

    public string? GetParentFullName(string path) => Directory.GetParent(path)?.FullName;

    public byte[] ReadAllBytes(string path) => File.ReadAllBytes(path);
    
    public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken ct) => File.ReadAllBytesAsync(path, ct);
  }
}