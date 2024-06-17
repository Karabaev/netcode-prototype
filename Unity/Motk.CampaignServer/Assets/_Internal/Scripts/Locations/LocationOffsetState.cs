using UnityEngine;

namespace Motk.CampaignServer.Locations
{
  public class LocationOffsetState
  {
    public Vector3 Offset { get; }

    public LocationOffsetState(Vector3 offset) => Offset = offset;
  }
}