using System;
using UnityEngine;
using PlayerInput;
using UnityEngine.InputSystem;

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

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Default.Dash.performed += OnDashPerformed;
        _controls.Default.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        _controls.Disable();
        _controls.Default.Dash.performed -= OnDashPerformed;
        _controls.Default.Jump.performed -= OnJumpPerformed;
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
}
