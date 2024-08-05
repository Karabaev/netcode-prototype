using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Motk.Descriptors.FileSystem;

namespace Motk.Client.Tests.Descriptors.StubTypes
{
  public class FileSystemOperationsStub : IFileSystemOperations
  {
    public static readonly Dictionary<string, byte[]> FileInfos = new()
    {
      { DescriptorRootDirectoryProviderStub.RootDirectory + "/CombatArenas/Arena1/CombatArena.json", new byte[] { 0, 1, 2 } },
      { DescriptorRootDirectoryProviderStub.RootDirectory + "/CombatArenas/Arena1/CombatArenaVisual.json", new byte[] { 3, 4, 5 } },
      { DescriptorRootDirectoryProviderStub.RootDirectory + "/CombatArenas/Arena2/CombatArena.json", new byte[] { 3, 4, 5 } },
      { DescriptorRootDirectoryProviderStub.RootDirectory + "/CombatArenas/Arena2/CombatArenaVisual.json", new byte[] { 3, 4, 5 } },
      { DescriptorRootDirectoryProviderStub.RootDirectory + "/CombatArenas/Arena3/CombatArenaVisual.json", new byte[] { 9, 9, 9 } },
      { DescriptorRootDirectoryProviderStub.RootDirectory + "/Units/Unit1/Unit.json", new byte[] { 3, 4, 5 } },
      { DescriptorRootDirectoryProviderStub.RootDirectory + "/Units/Unit2/UnitVisual.json", new byte[] { 3, 4, 5 } },
    };

    public IEnumerable<string> EnumerateFiles(string root, string pattern, SearchOption searchOption) => FileInfos.Keys;

    public string? GetParentFullName(string path) => Path.GetDirectoryName(path);

    public byte[] ReadAllBytes(string path) => FileInfos[path];
    public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken ct) => Task.FromResult(FileInfos[path]);
  }
}