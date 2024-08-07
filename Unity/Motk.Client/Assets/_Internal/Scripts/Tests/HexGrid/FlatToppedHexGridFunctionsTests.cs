using Mork.HexGrid.Render.Unity.Functions;
using Motk.HexGrid.Core.Descriptors;
using NUnit.Framework;
using UnityEngine;

namespace Motk.Client.Tests.HexGrid
{
  public class FlatToppedHexGridFunctionsTests
  {
    private readonly FlatToppedHexGridFunctions _functions = new();
    
    [TestCase(0.5f)]
    [TestCase(1.0f)]
    [TestCase(5.0f)]
    [TestCase(100.99f)]
    public void GetLocalCorner(float radius)
    {
      for (var i = 0; i < 6; i++)
      {
        var result = _functions.GetLocalCorner(i, radius);
        var expected = CalculateCorner(i, radius);
        Assert.AreEqual(expected.x, result.x, 0.0001f);
        Assert.AreEqual(expected.y, result.y, 0.0001f);
        Assert.AreEqual(expected.z, result.z, 0.0001f);
      }
    }

    [Test]
    public void HexCoordinatesToLocal()
    {
      var radius = 1.0f;
      var coordinates = new HexCoordinates(0, 0);
      var result = _functions.ToLocal(coordinates, radius);
      var expected = Vector3.zero;
      Assert.AreEqual(expected.x, result.x, 0.0001f);
      Assert.AreEqual(expected.y, result.y, 0.0001f);
      Assert.AreEqual(expected.z, result.z, 0.0001f);
      
      coordinates = new HexCoordinates(1, 0);
      result = _functions.ToLocal(coordinates, radius);
      expected = new Vector3(1.5f, 0.0f, 0.866f);
      Assert.AreEqual(expected.x, result.x, 0.0001f);
      Assert.AreEqual(expected.y, result.y, 0.0001f);
      Assert.AreEqual(expected.z, result.z, 0.0001f);
      
      radius = 5.0f;
      coordinates = new HexCoordinates(-5, -5);
      result = _functions.ToLocal(coordinates, radius);
      expected = new Vector3(-37.5f, 0.0f, -64.9519f);
      Assert.AreEqual(expected.x, result.x, 0.0001f);
      Assert.AreEqual(expected.y, result.y, 0.0001f);
      Assert.AreEqual(expected.z, result.z, 0.0001f);
    }
    
    private Vector3 CalculateCorner(int index, float radius)
    {
      var innerRadius = radius * Mathf.Sqrt(3.0f) * 0.5f;

      var corners = new Vector3[]
      {
        new(radius, 0f, 0.0f),
        new(radius * 0.5f, 0.0f, -innerRadius),
        new(-radius * 0.5f, 0.0f, -innerRadius),
        new(-radius, 0f, 0.0f),
        new(-radius * 0.5f, 0f, innerRadius),
        new(radius * 0.5f, 0.0f, innerRadius)
      };
      return corners[index];
    }
  }
}