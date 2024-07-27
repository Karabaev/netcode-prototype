using UnityEngine;

namespace Motk.Client.Combat.Grid.Hex.View
{
  public static class HexMetrics
  {
    public const float OuterRadius = 0.5f;
    public const float InnerRadius = OuterRadius * 0.866025404f;

    public static readonly Vector3[] Corners =
    {
      new(0f, 0f, OuterRadius),
      new(InnerRadius, 0f, 0.5f * OuterRadius),
      new(InnerRadius, 0f, -0.5f * OuterRadius),
      new(0f, 0f, -OuterRadius),
      new(-InnerRadius, 0f, -0.5f * OuterRadius),
      new(-InnerRadius, 0f, 0.5f * OuterRadius),
      new(0f, 0f, OuterRadius)
    };
  }
}