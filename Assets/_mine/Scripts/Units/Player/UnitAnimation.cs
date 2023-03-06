using System;
using System.Collections;
using UnityEngine;

namespace _mine.Scripts.Units.Player
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private ControleManager _controller;
        [SerializeField] private CharacterController _charController;
        [SerializeField] private Animator _animator;
        [SerializeField] private MovementSettings _moveSettings;
        
        private Vector2 _input;
        private bool _isDashing = false;
        private bool _isJumping = false;
        private Coroutine _dashingRoutine = null;
        
        
        private void Awake()
        {
            _controller.OnJumpEvent += Jump;
            _controller.OnDashEvent += Dash;
        }

        private void Jump()
        {
            if (_charController.isGrounded)
            {
                _isJumping = true;
                _animator.SetTrigger("isJumping");
            }
        }

        private void Dash()
        {
            if (_dashingRoutine != null) 
                return;
        
            _dashingRoutine = StartCoroutine(StopDash());
            _animator.SetTrigger("Dashing");
        }
        
        IEnumerator StopDash()
        {
            _isDashing = true;
            
            yield return new WaitForSeconds(_moveSettings.DashDuration);
            _isDashing = false;
            yield return new WaitForSeconds(_moveSettings.DashCoolDown - _moveSettings.DashDuration);
            _dashingRoutine = null;
        }


        private void Update()
        {
            if (_isDashing) return;
            
            _input = _controller.GetMovementVector();

            _animator.SetBool("isRunning", _input.magnitude > 0);
            _animator.SetBool("isJumping", !_charController.isGrounded);
        }
    }
}