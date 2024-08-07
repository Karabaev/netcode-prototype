using System;
using System.Linq;
using Mork.HexGrid.Render.Unity;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;
using NUnit.Framework;
using UnityEngine;

namespace Motk.Client.Tests.HexGrid
{
  public class HexDirectionTests
  {
    [Test]
    public void HexDirectionOpposite()
    {
      Assert.AreEqual(HexDirection.SW, HexDirection.NE.Opposite());
      Assert.AreEqual(HexDirection.W, HexDirection.E.Opposite());
      Assert.AreEqual(HexDirection.NW, HexDirection.SE.Opposite());
      Assert.AreEqual(HexDirection.NE, HexDirection.SW.Opposite());
      Assert.AreEqual(HexDirection.E, HexDirection.W.Opposite());
      Assert.AreEqual(HexDirection.SE, HexDirection.NW.Opposite());
    }

    [Test]
    public void HexDirectionToWorld()
    {
      var allDirections = Enum.GetValues(typeof(HexDirection)).Cast<HexDirection>();
      
      foreach (var direction in allDirections)
      {
        var expected = (int)direction * 60.0f + 30.0f;

        var delta = Vector3.Distance(new Vector3(0.0f, expected, 0.0f), direction.ToWorld().eulerAngles);
        Assert.AreEqual(0, delta, 0.0001f);
      }
    }

    [TestCase(0.0f)]
    [TestCase(30.0f)]
    [TestCase(90.0f)]
    [TestCase(50.0f)]
    [TestCase(150.0f)]
    [TestCase(210.0f)]
    [TestCase(220.0f)]
    [TestCase(270.0f)]
    [TestCase(330.0f)]
    public void WorldDirectionToHex(float y)
    {
      var world = Quaternion.Euler(0.0f, y, 0.0f);
      var expected = (HexDirection) (Mathf.RoundToInt((y - 30.0f) / 60f) % 6);
      Assert.AreEqual(expected, world.ToHex());
    }

    [Test]
    public void GetDirectionFromHexToHex()
    {
      var actual = HexRenderUtils.GetDirection(new HexCoordinates(), new HexCoordinates(0, 1));
      Assert.AreEqual(HexDirection.NE, actual);
      
      actual = HexRenderUtils.GetDirection(new HexCoordinates(), new HexCoordinates(1, 0));
      Assert.AreEqual(HexDirection.E, actual);
      
      actual = HexRenderUtils.GetDirection(new HexCoordinates(), new HexCoordinates(1, -1));
      Assert.AreEqual(HexDirection.SE, actual);
      
      actual = HexRenderUtils.GetDirection(new HexCoordinates(), new HexCoordinates(0, -1));
      Assert.AreEqual(HexDirection.SW, actual);
      
      actual = HexRenderUtils.GetDirection(new HexCoordinates(), new HexCoordinates(-1, 0));
      Assert.AreEqual(HexDirection.W, actual);
      
      actual = HexRenderUtils.GetDirection(new HexCoordinates(), new HexCoordinates(-1, 1));
      Assert.AreEqual(HexDirection.NW, actual);
    }
  }
}