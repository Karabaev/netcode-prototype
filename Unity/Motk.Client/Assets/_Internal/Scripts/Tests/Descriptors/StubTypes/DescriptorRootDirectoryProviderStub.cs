using Motk.Descriptors.FileSystem;

namespace Motk.Client.Tests.Descriptors.StubTypes
{
  public class DescriptorRootDirectoryProviderStub : IDescriptorsRootDirectoryProvider
  {
    public const string RootDirectory = "root";
    
    public string GetRootDirectory() => RootDirectory;
  }
}