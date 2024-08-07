using Mork.HexGrid.Render.Unity.Functions;
using NUnit.Framework;
using UnityEngine;

namespace Motk.Client.Tests.HexGrid
{
  public class PointyToppedHexGridFunctionsTests
  {
    private readonly PointyToppedHexGridFunctions _functions = new();
    
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
    
    private Vector3 CalculateCorner(int index, float radius)
    {
      var innerRadius = radius * Mathf.Sqrt(3.0f) * 0.5f;

      var corners = new Vector3[]
      {
        new(innerRadius, 0f, -0.5f * radius),
        new(0f, 0f, -radius),
        new(-innerRadius, 0f, -0.5f * radius),
        new(-innerRadius, 0f, 0.5f * radius),
        new(0f, 0f, radius),
        new(innerRadius, 0f, 0.5f * radius)
      };
      return corners[index];
    }
  }
}