using System.Collections.Generic;
using Motk.CampaignServer.Matches.States;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer;

namespace Motk.CampaignServer.DebugSystem
{
  public class ActorsGizmos : MonoBehaviour
  {
    private IObjectResolver? _objectResolver;

    private readonly Dictionary<int, Color> _matchColors = new();

    public void Construct(IObjectResolver objectResolver)
    {
      _objectResolver = objectResolver;
    }

    private void OnDrawGizmos()
    {
      if (_objectResolver == null) return;
      
      foreach (var (matchId, matchState) in _objectResolver.Resolve<MatchesState>().Matches)
      {
        if (!_matchColors.TryGetValue(matchId, out var color))
        {
          color = Random.ColorHSV(0, 1.0f, 0, 1.0f);
          _matchColors.Add(matchId, color);
        }
        Gizmos.color = color;

        var locationState = matchState.Scope.Container.Resolve<CampaignLocationState>();
        foreach (var (clientId, actorState) in locationState.Actors)
        {
          Gizmos.DrawCube(actorState.Position.Value + Vector3.up, new Vector3(1, 2, 1));
        }
      }
    }
  }
}