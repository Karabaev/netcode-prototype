using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Motk.Client.Core.InputSystem
{
  public class InputController : MonoBehaviour
  {
    private InputState _state = null!;

    private void Awake() => enabled = false;

    [Inject]
    public void Construct(InputState state)
    {
      _state = state;
      enabled = true;
    }

    private void Update()
    {
      if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
      {
        _state.MainMouseButtonClicked.Invoke(Input.mousePosition);
      }
    }
    
    private bool IsPointerOverUI()
    {
      if(!EventSystem.current.IsPointerOverGameObject())
        return false;
      
      var pointerEventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

      var results = new List<RaycastResult>();
      EventSystem.current.RaycastAll(pointerEventData, results);

      return results.Count > 0;
    }
  }
}