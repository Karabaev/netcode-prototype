using System;
using System.Threading.Tasks;

namespace Motk.Descriptors.Serialization
{
  public interface IDescriptorSerializer
  {
    T? Deserialize<T>(string str);
    T? Deserialize<T>(byte[] bytes);
    object? Deserialize(string str, Type type);
    object? Deserialize(byte[] bytes, Type type);
    ValueTask<T?> DeserializeAsync<T>(string str);
    ValueTask<T?> DeserializeAsync<T>(byte[] bytes);
    ValueTask<object?> DeserializeAsync(byte[] bytes, Type type);
    string Serialize(object obj);
    ValueTask<string> SerializeAsync(object obj);
    ValueTask<byte[]> SerializeToBytesAsync(object obj);
    byte[] SerializeToBytes(object obj);
  }
}