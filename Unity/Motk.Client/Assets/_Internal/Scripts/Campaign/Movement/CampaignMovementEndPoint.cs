using Cysharp.Threading.Tasks;
using Motk.Shared.Campaign.Actors;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Campaign.Movement;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Motk.Client.Campaign.Movement
{
  
  // todokmo переделать
  public class CampaignMovementEndPoint
  {
    private readonly NetworkManager _networkManager;
    private readonly ActorMovementLogic _actorMovementLogic;
    private readonly CampaignActorsState _actorsState;

    public CampaignMovementEndPoint(NetworkManager networkManager, ActorMovementLogic actorMovementLogic,
      CampaignActorsState actorsState)
    {
      _networkManager = networkManager;
      _actorMovementLogic = actorMovementLogic;
      _actorsState = actorsState;

      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(nameof(ActorMovementStartedCommand), OnActorMovementStated);
    }

    public void Send(Vector3 destination)
    {
      var request = new FindPathRequest { Destination = destination };
      using var fastWriter = new FastBufferWriter(1024, Allocator.Temp);
      
      fastWriter.WriteValueSafe(request);
      _networkManager.CustomMessagingManager.SendNamedMessage(nameof(FindPathRequest), NetworkManager.ServerClientId, fastWriter);
    }

    private void OnActorMovementStated(ulong senderclientid, FastBufferReader messagepayload)
    {
      messagepayload.ReadValueSafe(out ActorMovementStartedCommand command);
      var actorState = _actorsState.Actors.Require(command.PlayerId);
      _actorMovementLogic.MoveAsync(actorState, command.Path).Forget();
    }
  }
}