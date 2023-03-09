using System;
using UnityEngine;
using UnityEngine.UI;
public class LoaderCallback : MonoBehaviour
{
    [SerializeField] private Slider _sliderBar;
    private bool _isFirstUpdate = true;

    private void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            Loader.LoaderCallBack();
        }

        _sliderBar.value = Loader.GetLoadingProgress();
    }
}
