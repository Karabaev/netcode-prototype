using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Motk.Client.Core.Descriptors.Serialization;
using NUnit.Framework;

namespace Motk.Client.Tests.Descriptors
{
  public class JsonDescriptorSerializerTests
  {
    private JsonDescriptorSerializer _serializer = null!;
    private SerializedStub _testObject = null!;
    private string _serializedTestObject = null!;
    
    [SetUp]
    public void SetUp()
    {
      _serializer = new JsonDescriptorSerializer(new JsonSerializerOptions
      {
        IncludeFields = true,
        ReadCommentHandling  = JsonCommentHandling.Disallow,
        WriteIndented = true
      });
      _testObject = new SerializedStub
      {
        Int = 5,
        Float = 99.55f,
        String = "This is string",
        IntArray = new[] { 1, 2, 3, 2, 1 },
        Nested = new NestedSerializedStub
        {
          Int = 10,
          Float = 45.55f,
          String = "Nested string"
        }
      };
      
      _serializedTestObject = 
        @"{
  ""Int"": 5,
  ""Float"": 99.5500031,
  ""String"": ""This is string"",
  ""IntArray"": [
    1,
    2,
    3,
    2,
    1
  ],
  ""Nested"": {
    ""Int"": 10,
    ""Float"": 45.5499992,
    ""String"": ""Nested string""
  }
}";
    }
    
    [Test]
    public void SerializeToString()
    {
      var actualStr = _serializer.Serialize(_testObject);
      Assert.AreEqual(_serializedTestObject, actualStr);
    }

    [Test]
    public void SerializeToBytes()
    {
      var actualBytes = _serializer.SerializeToBytes(_testObject);
      var expectedBytes = Encoding.UTF8.GetBytes(_serializedTestObject);
      Assert.AreEqual(expectedBytes.Length, actualBytes.Length);

      for (var i = 0; i < expectedBytes.Length; i++)
        Assert.AreEqual(expectedBytes[i], actualBytes[i]);
    }

    [Test]
    public async Task SerializeToStringAsync()
    {
      var actualStr = await _serializer.SerializeAsync(_testObject);
      Assert.AreEqual(_serializedTestObject, actualStr);
    }

    [Test]
    public async Task SerializeToBytesAsync()
    {
      var actualBytes = await _serializer.SerializeToBytesAsync(_testObject);
      var expectedBytes = Encoding.UTF8.GetBytes(_serializedTestObject);
      Assert.AreEqual(expectedBytes.Length, actualBytes.Length);

      for (var i = 0; i < expectedBytes.Length; i++)
        Assert.AreEqual(expectedBytes[i], actualBytes[i]);
    }
    
    [Test]
    public void DeserializeFromString()
    {
      var actualObj = _serializer.Deserialize<SerializedStub>(_serializedTestObject);
      Assert.AreEqual(_testObject, actualObj);
      
      actualObj = (SerializedStub) _serializer.Deserialize(_serializedTestObject, typeof(SerializedStub))!;
      Assert.AreEqual(_testObject, actualObj);
    }

    [Test]
    public void DeserializeFromBytes()
    {
      var bytes = Encoding.UTF8.GetBytes(_serializedTestObject);
      var actualObj = _serializer.Deserialize<SerializedStub>(bytes);
      Assert.AreEqual(_testObject, actualObj);
      
      actualObj = (SerializedStub) _serializer.Deserialize(bytes, typeof(SerializedStub))!;
      Assert.AreEqual(_testObject, actualObj);
    }

    [Test]
    public async Task DeserializeFromStringAsync()
    {
      var actualObj = await _serializer.DeserializeAsync<SerializedStub>(_serializedTestObject);
      Assert.AreEqual(_testObject, actualObj);
    }

    [Test]
    public async Task DeserializeFromBytesAsync()
    {
      var bytes = Encoding.UTF8.GetBytes(_serializedTestObject);
      var actualObj = await _serializer.DeserializeAsync<SerializedStub>(bytes);
      Assert.AreEqual(_testObject, actualObj);
      
      actualObj = (SerializedStub) (await _serializer.DeserializeAsync(bytes, typeof(SerializedStub)))!;
      Assert.AreEqual(_testObject, actualObj);
    }
  }

  [Serializable]
  public class SerializedStub : IEquatable<SerializedStub>
  {
    public int Int;
    public float Float;
    public string String = null!;
    public int[] IntArray = null!;
    public NestedSerializedStub Nested = null!;
    
    public bool Equals(SerializedStub? other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Int == other.Int 
             && Float.Equals(other.Float)
             && String == other.String
             && IntArray.All(i => other.IntArray.Contains(i))
             && Nested.Equals(other.Nested);
    }

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((SerializedStub)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Int, Float, String, IntArray, Nested);
  }

  public class NestedSerializedStub : IEquatable<NestedSerializedStub>
  {
    public int Int;
    public float Float;
    public string String = null!;

    public bool Equals(NestedSerializedStub? other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Int == other.Int && Float.Equals(other.Float) && String == other.String;
    }

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((NestedSerializedStub)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Int, Float, String);
  }
}