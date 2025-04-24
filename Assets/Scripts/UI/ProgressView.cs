using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _valueField;
    public string textAfterNumber;

    public void Set(float value, float maxValue)
    {
        float valueToShow = value / maxValue;

        _image.fillAmount = valueToShow;

        string valueToShowText = value.ToString();
        _valueField.text = valueToShowText + textAfterNumber;
    }
}