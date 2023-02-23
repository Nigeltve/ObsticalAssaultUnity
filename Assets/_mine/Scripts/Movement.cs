using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Helper;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private CharacterController _charController;
    [SerializeField] private ControleManager _controller;    
    
    [Separator("Basic Movement")]
    [SerializeField] private float _movementspeed = 5f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float _jumpHeight = 15f;
    [SerializeField] private float smoothTime = 0.05f;
    
    [Separator("Dash Movement")] 
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCoolDown = 3f;
    [SerializeField] private float _dashStrength = 3f;
    
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
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    
    private void ApplyMovement()
    {
        if(_isDashing)
            _charController.Move(_direction * (_movementspeed * _dashStrength * Time.deltaTime));
        else
            _charController.Move(_direction * (_movementspeed  * Time.deltaTime));
    }
    
    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
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
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(_dashCoolDown - _dashDuration);
        _dashRoutine = null;
    }

    private void Jump()
    {
        if (!IsGrounded()) return;

        _velocity += _jumpHeight;
    }
    
    private bool IsGrounded() => _charController.isGrounded;


}
