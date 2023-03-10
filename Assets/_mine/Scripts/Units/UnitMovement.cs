using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Helper;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _charController;
    [SerializeField] private ControleManager _controller;

    [SerializeField] private MovementSettings _moveSettings;
    
    private Vector3 _input;
    private Vector3 _direction;
    private float _velocity;
    private float _gravity = -9.81f;
    private float _currentVelocity;


    private bool _isDashing = false;
    private Coroutine _dashRoutine = null;

    private void OnEnable()
    {
        _controller.OnJumpEvent += Jump;
        _controller.OnDashEvent += Dash;
    }

    private void OnDisable()
    {
        _controller.OnJumpEvent -= Jump;
        _controller.OnDashEvent -= Dash;
    }

    private void Update()
    {
        GetMovement();
        
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }

    void GetMovement()
    {
        _input = _controller.GetMovementVector();
        
        _direction = new Vector3(_input.x, 0, _input.y);
        _direction.ToIso();
    }

    void ApplyRotation()
    {
        // if no input then done do anything
        if (_input.magnitude == 0) return;
        
        // find where the player wants to move to;
        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, _moveSettings.TurnTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    
    private void ApplyMovement()
    {
        if(_isDashing)
            _charController.Move(_direction * (_moveSettings.MovementSpeed * _moveSettings.DashStrength * Time.deltaTime));
        else
            _charController.Move(_direction * (_moveSettings.MovementSpeed  * Time.deltaTime));
    }
    
    private void ApplyGravity()
    {
        if (_isDashing)
            return;
        
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * _moveSettings.GravityMultiplier * Time.deltaTime;
        }
        
        _direction.y = _velocity;
    }

    private void Dash()
    {
        if (_dashRoutine != null) 
            return;

        _dashRoutine = StartCoroutine(StopDash());
    }

    IEnumerator StopDash()
    {
        _isDashing = true;

        float t = 0;

        while (t < _moveSettings.DashDuration)
        {
            t += Time.deltaTime;
            
            Debug.Log($"[dashing] {t}");
            _direction.y = 0;
            yield return null;
        }
        
        //yield return new WaitForSeconds(_moveSettings.DashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(_moveSettings.DashCoolDown - _moveSettings.DashDuration);
        _dashRoutine = null;
    }

    private void Jump()
    {
        if (!IsGrounded()) return;

        _velocity += _moveSettings.JumpHeight;
    }
    
    private bool IsGrounded() => _charController.isGrounded;


}
