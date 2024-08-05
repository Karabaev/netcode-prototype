using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Motk.Descriptors.FileSystem
{
  public interface IFileSystemOperations
  {
    IEnumerable<string> EnumerateFiles(string root, string pattern, SearchOption searchOption);
    string? GetParentFullName(string path);
    byte[] ReadAllBytes(string path);
    Task<byte[]> ReadAllBytesAsync(string path, CancellationToken ct);
  }
}