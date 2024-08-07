﻿using JetBrains.Annotations;
using Motk.Client.Campaign.Actors.Descriptors;
using Motk.Client.Campaign.Actors.Views;
using Motk.Shared.Campaign.Actors.States;
using UnityEngine;

namespace Motk.Client.Campaign.Actors.Services
{
  [UsedImplicitly]
  public class CampaignActorViewFactory
  {
    private readonly CharactersRegistry _charactersRegistry;

    public CampaignActorViewFactory(CharactersRegistry charactersRegistry) => _charactersRegistry = charactersRegistry;

    public CampaignActorView Create(string characterId, CampaignActorState state)
    {
      var prefab = _charactersRegistry.Entries[characterId].Prefab;
      var rotation = Quaternion.Euler(Vector3.one * state.EulerY.Value);
      var instance = Object.Instantiate(prefab, state.Position.Value, rotation);
      instance.Construct(state);
      return instance;
    }
  }
}