using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Motk.Descriptors.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Motk.Client.Core.Descriptors.Serialization
{
  public class JsonDescriptorSerializer : IDescriptorSerializer
  {
    private readonly JsonSerializerOptions _options;
    
    public T? Deserialize<T>(string str) => JsonSerializer.Deserialize<T>(str, _options);

    public T? Deserialize<T>(byte[] bytes) => JsonSerializer.Deserialize<T>(bytes, _options);

    public object? Deserialize(string str, Type type) => JsonSerializer.Deserialize(str, type, _options);

    public object? Deserialize(byte[] bytes, Type type) => JsonSerializer.Deserialize(bytes, type, _options);
    
    public ValueTask<T> DeserializeAsync<T>(string str)
    {
      using var stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
      return JsonSerializer.DeserializeAsync<T>(stream, _options);
    }

    public ValueTask<T> DeserializeAsync<T>(byte[] bytes)
    {
      using var stream = new MemoryStream(bytes);
      return JsonSerializer.DeserializeAsync<T>(stream, _options);
    }

    public ValueTask<object?> DeserializeAsync(byte[] bytes, Type type)
    {
      using var stream = new MemoryStream(bytes);
      return JsonSerializer.DeserializeAsync(stream, type, _options);
    }

    public string Serialize(object obj) => JsonSerializer.Serialize(obj, _options);
    
    public async ValueTask<string> SerializeAsync(object obj)
    {
      using var stream = new MemoryStream();
      await JsonSerializer.SerializeAsync(stream, obj, _options);
      using var reader = new StreamReader(stream);
      return await reader.ReadToEndAsync();
    }

    public byte[] SerializeToBytes(object obj) => JsonSerializer.SerializeToUtf8Bytes(obj, _options);
    
    public JsonDescriptorSerializer(JsonSerializerOptions options) => _options = options;
  }
}