using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalMoney;
    [SerializeField] private TextMeshProUGUI _incomePerMinute;
    [SerializeField] private BusinessSettingsView _businessSettingsView;


    private void OnEnable()
    {
        BusinessBehaviour.OnBusinessClicked += _businessSettingsView.Activate;
    }

    private void OnDisable()
    {
        BusinessBehaviour.OnBusinessClicked -= _businessSettingsView.Activate;
    }

    private void Update()
    {
        _totalMoney.text = GameData.totalMoney.GetComaFormat();
        _incomePerMinute.text = GameData.totalIncomePerMinute.FormatLargeNumber() + " /min";
    }
}