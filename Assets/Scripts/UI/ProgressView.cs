using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _valueField;
    public string textAfterNumber;
    public float maxValue;
    
    public void Set(float value)
    {
        Set(value, maxValue);
    }

    public void Set(float value, float maxValue)
    {
        float valueToShow = value / maxValue;

        _image.fillAmount = 1/maxValue*valueToShow;

        string valueToShowText = valueToShow.ToString();
        _valueField.text = valueToShowText + textAfterNumber;
    }
}