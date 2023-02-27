using System;
using UnityEngine;
using PlayerInput;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControleManager : MonoBehaviour
{
    private ControlMap _controls;
    private Vector2 _movementVector;

    public Action OnDashEvent;
    public Action OnJumpEvent;
    
    private void Awake()
    {
        _controls = new ControlMap();
    }
    
    EventSystem system;
 
    void Start()
    {
        system = EventSystem.current;
   
    }
    
    
    private void OnEnable()
    {
        _controls.Enable();
        _controls.Default.Dash.performed += OnDashPerformed;
        _controls.Default.Jump.performed += OnJumpPerformed;
        _controls.Default.Tab.performed += OnTabPressed;
    }

    private void OnDisable()
    {
        _controls.Disable();
        _controls.Default.Dash.performed -= OnDashPerformed;
        _controls.Default.Jump.performed -= OnJumpPerformed;
        _controls.Default.Tab.performed -= OnTabPressed;
    }

    private void Update()
    {
        _movementVector = _controls.Default.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMovementVector()
    {
        return _movementVector;
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        OnDashEvent?.Invoke();
    }
    
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        OnJumpEvent?.Invoke();
    }

    private void OnTabPressed(InputAction.CallbackContext ctx)
    {
        try
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(
                        new PointerEventData(system)); //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
        }
        catch
        {
            Debug.Log("[Tab] Nothing to tab to");
        }
    }
}
