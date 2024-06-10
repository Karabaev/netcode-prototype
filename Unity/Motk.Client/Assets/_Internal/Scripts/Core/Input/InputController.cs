using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Motk.Client.Core.Input
{
  public class InputController : MonoBehaviour
  {
    private InputState _state = null!;

    private void Awake() => enabled = false;

    public void Construct(InputState state)
    {
      _state = state;
      enabled = true;
    }

    private void Update()
    {
      if (UnityEngine.Input.GetMouseButtonDown(0) && !IsPointerOverUI())
      {
        _state.MainMouseButtonClicked.Invoke(UnityEngine.Input.mousePosition);
      }
    }
    
    private bool IsPointerOverUI()
    {
      if(!EventSystem.current.IsPointerOverGameObject())
        return false;
      
      var pointerEventData = new PointerEventData(EventSystem.current) { position = UnityEngine.Input.mousePosition };

      var results = new List<RaycastResult>();
      EventSystem.current.RaycastAll(pointerEventData, results);

      return results.Count > 0;
    }
  }
}