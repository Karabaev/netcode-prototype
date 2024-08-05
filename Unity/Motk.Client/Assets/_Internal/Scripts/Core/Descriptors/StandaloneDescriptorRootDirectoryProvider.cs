using Motk.Descriptors.FileSystem;

namespace Motk.Client.Core.Descriptors
{
  public class StandaloneDescriptorRootDirectoryProvider : IDescriptorsRootDirectoryProvider
  {
    public string GetRootDirectory()
    {
      return UnityEngine.Application.persistentDataPath;
    }
  }
}