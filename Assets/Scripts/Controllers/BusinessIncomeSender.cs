using TMPro;
using UnityEngine;

public class BusinessIncomeSender : MonoBehaviour, IClickable
{
    [SerializeField] private BusinessBehaviour _business;
    [SerializeField] private GameObject _coinObj;
    [SerializeField] private TextMeshPro _valueField;
    [SerializeField] private double _manyToShowCoin;

    public void OnClicked()
    {
        _business.SendIncome();
    }

    private void Update()
    {
        if(_business.AccumulatedMoney > _manyToShowCoin)
        {
            _coinObj.SetActive(true);
            _valueField.gameObject.SetActive(true);
            _valueField.text = _business.AccumulatedMoney.FormatLargeNumber() + " $";
        }
        else
        {
            _coinObj.SetActive(false);
            _valueField.gameObject.SetActive(false);
        }
    }
}
