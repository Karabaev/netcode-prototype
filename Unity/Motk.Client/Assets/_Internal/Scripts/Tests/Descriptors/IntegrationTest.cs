using System.IO;
using System.Linq;
using Motk.Client.Core.Descriptors;
using NUnit.Framework;

namespace Motk.Client.Tests.Descriptors
{
  [TestFixture(Category = "Integration")]
  public class DescriptorsIntegrationTest
  {
    [Test]
    public void DescriptorsDirectoryExists()
    {
      var provider = new EditorDescriptorsRootDirectoryProvider();
      var path = provider.GetRootDirectory();
      Assert.True(Directory.Exists(path));
      var descriptors = Directory.EnumerateFiles(path, "*.descriptor", SearchOption.AllDirectories);
      Assert.True(descriptors.Any());
    }
  }
}