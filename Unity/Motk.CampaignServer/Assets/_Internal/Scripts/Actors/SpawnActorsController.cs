using System;
using JetBrains.Annotations;
using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Campaign.Actors.States;
using Unity.Collections;
using Unity.Netcode;

namespace Motk.CampaignServer.Actors
{
  [UsedImplicitly]
  public class SpawnActorsController : IDisposable
  {
    private readonly CampaignActorsState _state;
    private readonly NetworkManager _networkManager;

    public SpawnActorsController(CampaignActorsState state, NetworkManager networkManager)
    {
      _state = state;
      _networkManager = networkManager;
      _state.Actors.ItemRemoved += State_OnActorRemoved;
    }

    public void Dispose()
    {
      _state.Actors.ItemRemoved -= State_OnActorRemoved;
    }

    private void State_OnActorRemoved(ulong clientId, CampaignActorState _)
    {
      var command = new PlayerActorDespawnedCommand();
      using var writer = new FastBufferWriter(1024, Allocator.Temp);
      
      writer.WriteValueSafe(command);
      _networkManager.CustomMessagingManager.SendNamedMessageToAll(nameof(PlayerActorDespawnedCommand), writer);
    }
  }
}