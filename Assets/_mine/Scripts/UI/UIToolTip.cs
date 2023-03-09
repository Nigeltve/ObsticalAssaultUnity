using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;


[ExecuteInEditMode]
public class UIToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private int WrapLength = 500;
    
    private void Update()
    {
        _layoutElement.preferredWidth = WrapLength;
        _layoutElement.enabled = Mathf.Max(_header.preferredWidth, _content.preferredWidth) >= _layoutElement.preferredWidth;
    }

    public void SetHeaderText(string text)
    {
        _header.text = text;
    }

    public void SetContentText(string text)
    {
        _content.text = text;
    }

}
