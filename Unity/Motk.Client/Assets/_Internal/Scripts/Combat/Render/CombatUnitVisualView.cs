using com.karabaev.utilities.unity.GameKit;
using UnityEngine;

namespace Motk.Client.Combat.Render
{
  public class CombatUnitVisualView : GameKitComponent
  {
    private CombatUnitVisualState _visualState = null!;
    
    private void Awake() => enabled = false;

    public void Construct(CombatUnitVisualState visualState)
    {
      _visualState = visualState;
      _visualState.Position.Changed += State_OnPositionChanged;
      _visualState.Rotation.Changed += State_OnRotationChanged;
      State_OnPositionChanged(Vector3.zero, _visualState.Position.Value);
      State_OnRotationChanged(Quaternion.identity, _visualState.Rotation.Value);
      
      enabled = true;
    }

    private void OnDestroy()
    {
      _visualState.Position.Changed -= State_OnPositionChanged;
      _visualState.Rotation.Changed -= State_OnRotationChanged;
    }

    private void State_OnPositionChanged(Vector3 oldValue, Vector3 newValue) => transform.position = newValue;

    private void State_OnRotationChanged(Quaternion oldValue, Quaternion newValue) => transform.rotation = newValue;
  }
}