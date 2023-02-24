using System;
using System.Collections;
using System.Collections.Generic;
using Helper;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISprintCoolDown : MonoBehaviour
{
    [SerializeField] private ControleManager _controller;
    [SerializeField] private Image _coolDownImage;
    [SerializeField] private MovementSettings _moveSettings;

    private Coroutine FillRoutine = null;
    
    private void OnEnable()
    {
        _controller.OnDashEvent += Dash;
    }

    private void OnDisable()
    {
        _controller.OnDashEvent -= Dash;
    }

    private void Dash()
    {
        if (FillRoutine == null) FillRoutine = StartCoroutine(Fill());
    }

    IEnumerator Fill()
    {
        float currTime = 0;
        while (currTime < _moveSettings.DashCoolDown)
        {
            _coolDownImage.fillAmount = currTime / _moveSettings.DashCoolDown;
            yield return null;
            currTime += Time.deltaTime;
        }

        _coolDownImage.fillAmount = 1;
        FillRoutine = null;
    }
}
