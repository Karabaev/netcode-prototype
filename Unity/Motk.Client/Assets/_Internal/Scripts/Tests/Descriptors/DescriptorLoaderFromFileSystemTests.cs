using System.Threading.Tasks;
using Motk.Client.Tests.Descriptors.StubTypes;
using Motk.Descriptors;
using Motk.Descriptors.FileSystem;
using NUnit.Framework;

namespace Motk.Client.Tests.Descriptors
{
  public class DescriptorLoaderFromFileSystemTests
  {
    private DescriptorLoaderFromFileSystem _loader = null!;
    
    [SetUp]
    public void SetUp()
    {
      _loader = new DescriptorLoaderFromFileSystem(new FileSystemOperationsStub(), new DescriptorRootDirectoryProviderStub());
    }
    
    [Test]
    public async Task LoadDescriptorsTest()
    {
      await _loader.LoadAsync(default);

      var category = "CombatArenas";
      var type = "CombatArena";
      var result = await _loader.GetDescriptorsAsync(category, type, default);
      Assert.AreEqual(2, result.Count);
      foreach (var descriptorData in result)
        AssertData(category, type, descriptorData);
      
      type = "CombatArenaVisual";
      result = await _loader.GetDescriptorsAsync(category, type, default);
      Assert.AreEqual(3, result.Count);
      foreach (var descriptorData in result)
        AssertData(category, type, descriptorData);
      
      category = "Units";
      type = "Unit";
      result = await _loader.GetDescriptorsAsync(category, type, default);
      Assert.AreEqual(1, result.Count);
      foreach (var descriptorData in result)
        AssertData(category, type, descriptorData);
      
      type = "UnitVisual";
      result = await _loader.GetDescriptorsAsync(category, type, default);
      Assert.AreEqual(1, result.Count);
      foreach (var descriptorData in result)
        AssertData(category, type, descriptorData);
    }

    private void AssertData(string category, string type, DescriptorData result)
    {
      var path = DescriptorRootDirectoryProviderStub.RootDirectory + $"/{category}/{result.Id}/{type}.json";
      var expectedData = FileSystemOperationsStub.FileInfos[path];
      Assert.AreEqual(expectedData.Length, result.Data.Length);
      for (var i = 0; i < expectedData.Length; i++)
      {
        var expected = expectedData[i];
        var actual = result.Data[i];
        Assert.AreEqual(expected, actual);
      }
    }
  }
}