using System;
using System.Collections.Generic;
using com.karabaev.utilities.unity;
using JetBrains.Annotations;
using Motk.Shared.Locations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Motk.CampaignServer.Locations
{
  [UsedImplicitly]
  public class RoomLocationController : IDisposable
  {
    private readonly LocationsRegistry _locationsRegistry;
    private readonly CampaignLocationsState _locationsState;

    private readonly Dictionary<string, GameObject> _locations = new();

    public RoomLocationController(LocationsRegistry locationsRegistry, CampaignLocationsState locationsState)
    {
      _locationsRegistry = locationsRegistry;
      _locationsState = locationsState;
      _locationsState.RoomsToLocations.ItemAdded += State_OnRoomAdded;
      _locationsState.RoomsToLocations.ItemRemoved += State_OnRoomRemoved;
    }
    
    public void Dispose()
    {
      _locationsState.RoomsToLocations.ItemAdded -= State_OnRoomAdded;
      _locationsState.RoomsToLocations.ItemRemoved -= State_OnRoomRemoved;
    }
    
    private void State_OnRoomAdded(int roomId, CampaignLocationState newLocationState)
    {
      var locationId = newLocationState.LocationId;
      var locationDescriptor = _locationsRegistry.Entries[locationId];
      _locations[locationId] = Object.Instantiate(locationDescriptor.Prefab);
    }

    private void State_OnRoomRemoved(int roomId, CampaignLocationState oldLocationState)
    {
      _locations.Remove(oldLocationState.LocationId, out var location);
      location.DestroyObject();
    }
  }
}