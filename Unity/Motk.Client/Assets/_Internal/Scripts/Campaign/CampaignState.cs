using UnityEngine;

namespace Motk.Client.Campaign
{
  public class CampaignState
  {
    public string LocationId { get; set; } = null!;
    
    // todokmo переделать локации на сцены. Тогда не придется хранить GO локации для уничтожения
    public GameObject? LocationView { get; set; } 
  }
}