using System;
using com.karabaev.utilities.unity;
using Motk.CampaignServer.Matches.States;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Motk.CampaignServer.Matches
{
  public class MatchLifeCycleController : IStartable, IDisposable
  {
    private readonly LocationsRegistry _locationsRegistry;
    private readonly MatchState _matchState;

    private GameObject _locationObject = null!;

    public MatchLifeCycleController(LocationsRegistry locationsRegistry, MatchState matchState)
    {
      _locationsRegistry = locationsRegistry;
      _matchState = matchState;
    }

    public void Start()
    {
      var locationDescriptor = _locationsRegistry.Entries[_matchState.LocationId];
      _locationObject = Object.Instantiate(locationDescriptor.Prefab);
      // создать персов для игроков
    }

    public void Dispose()
    {
      _locationObject.DestroyObject();
    }
  }
}