using System;
using System.Threading;
using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Connection;
using Motk.Shared.Locations;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Motk.Client.Campaign.InputSystem
{
  [UsedImplicitly]
  public class AutomatedCampaignInputController : IDisposable
  {
    private const float MoveMinInterval = 0.5f;
    private const float MoveMaxInterval = 5.0f;

    private const float TransitionMinInterval = 10.0f;
    private const float TransitionMaxInterval = 60.0f;
    
    private readonly CampaignInputState _state;
    private readonly LocationsRegistry _locationsRegistry;
    private readonly ApplicationStateMachine _applicationStateMachine;
    private readonly CancellationTokenSource _cts;
    public AutomatedCampaignInputController(CampaignInputState state, LocationsRegistry locationsRegistry, ApplicationStateMachine applicationStateMachine)
    {
      _state = state;
      _locationsRegistry = locationsRegistry;
      _applicationStateMachine = applicationStateMachine;
      _cts = new CancellationTokenSource();
      UniTask.Void(MoveCycle, _cts.Token);
      UniTask.Void(TransitionCycle, _cts.Token);
    }

    void IDisposable.Dispose() => _cts.Cancel();

    private async UniTaskVoid MoveCycle(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        var point = GetPassablePoint();
        _state.GroundClicked.Invoke(point);

        var delay = Random.Range(MoveMinInterval, MoveMaxInterval);
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
      }
    }

    private Vector3 GetPassablePoint()
    {
      var pointFound = false;
      while (!pointFound)
      {
        var randomPoint = new Vector3(Random.Range(-10, 20), 0, Random.Range(-10, 20));
        pointFound = NavMesh.SamplePosition(randomPoint, out var hit, 3, NavMesh.AllAreas);
        if (pointFound)
        {
          return hit.position;
        }
      }

      return Vector3.zero;

    }

    private async UniTaskVoid TransitionCycle(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        var delay = Random.Range(TransitionMinInterval, TransitionMaxInterval);
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);

        MoveToRandomLocation();
      }
    }

    private void MoveToRandomLocation()
    {
      var (locationId, _) = _locationsRegistry.Entries.PickRandom();
      var stateContext = new EnterToLocationAppState.Context(locationId);
      _applicationStateMachine.EnterAsync<EnterToLocationAppState, EnterToLocationAppState.Context>(stateContext).Forget();
    }
  }
}