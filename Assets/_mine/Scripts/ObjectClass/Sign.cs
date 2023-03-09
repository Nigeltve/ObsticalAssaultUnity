using System;
using UnityEngine;


public class Sign : MonoBehaviour
{
    [SerializeField] private string _header;
    
    [SerializeField, Multiline] private string _content;
    [SerializeField] private UIToolTip _toolTip;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _toolTip.gameObject.SetActive(true);
            _toolTip.SetHeaderText(_header);
            _toolTip.SetContentText(_content);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _toolTip.gameObject.SetActive(false);
            _toolTip.SetHeaderText("");
            _toolTip.SetContentText("");
        }
    }
}
