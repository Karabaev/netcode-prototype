namespace Motk.Descriptors
{
  public class DescriptorData
  {
    public readonly string Id;
    public readonly byte[] Data;

    public DescriptorData(string id, byte[] data)
    {
      Id = id;
      Data = data;
    }
  }
}