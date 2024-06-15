using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Motk.Client.Core.InputSystem
{
  public class InputController : MonoBehaviour
  {
    private InputState _state = null!;
    
    private bool _auxDragging;
    private Vector2 _lastDragPosition;
    
    private Vector2 MousePosition => Input.mousePosition;

    private void Awake() => enabled = false;

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

      _state.MouseWheelAxis = Input.GetAxis("Mouse ScrollWheel");

      if (Input.GetMouseButtonDown(1) && !IsPointerOverUI()) 
        _auxDragging = true;

      if(Input.GetMouseButtonUp(1))
        _auxDragging = false;
      
      _state.AuxMouseButtonDragAxis = _auxDragging ? MousePosition - _lastDragPosition : Vector2.zero;
      _lastDragPosition = MousePosition;
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