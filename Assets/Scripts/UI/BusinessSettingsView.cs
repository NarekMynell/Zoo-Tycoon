using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessSettingsView : MonoBehaviour
{
    [SerializeField] private ProgressView _progressView;
    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _currentLevelIncomePerMinute;
    [SerializeField] private TextMeshProUGUI _upgradeCost;
    [SerializeField] private TextMeshProUGUI _nextLevelIncomeDelta;

    private BusinessBehaviour _business;
    

    private void Awake()
    {
        _upgradeBtn.onClick.AddListener(UpgradeLevel);
    }

    private void Update()
    {
        if(_business.HasUpgrade && GameData.totalMoney >= _business.UpgradeCost)
        {
            _upgradeBtn.interactable = true;
        }
        else
        {
            _upgradeBtn.interactable = false;
        }
    }

    private void OnDisable()
    {
        _business = null;
    }

    public void Activate(BusinessBehaviour business)
    {
        _business = business;
        if(_business.CurrentLevel == 0)
        {
            ActivateAsClosedBusiness();
        }
        else
        {
            SetForCurrentLevel();
        }
        gameObject.SetActive(true);
    }

    private void UpgradeLevel()
    {
        _business.UpgradeLevel();

        SetForCurrentLevel();
    }

    private void SetForCurrentLevel()
    {
        _currentLevelIncomePerMinute.text = _business.IncomePerMinute.FormatLargeNumber() + " $ /min";
        _header.text = _business.Name;

        if (_business.HasUpgrade)
        {
            _upgradeCost.gameObject.SetActive(true);
            _nextLevelIncomeDelta.gameObject.SetActive(true);
            _upgradeCost.text = _business.UpgradeCost.FormatLargeNumber() + " $";
            _nextLevelIncomeDelta.text = "+ " + (_business.NextLevelIncomePerMinute - _business.IncomePerMinute).FormatLargeNumber() + " $ /min";
        }
        else
        {
            _upgradeCost.gameObject.SetActive(false);
            _nextLevelIncomeDelta.gameObject.SetActive(false);
            _upgradeBtn.interactable = false;
        }

        _progressView.Set(_business.CurrentLevel, _business.LevelsCount);
    }

    private void ActivateAsClosedBusiness()
    {
        _header.text = "?????";
        _upgradeCost.gameObject.SetActive(true);
        _nextLevelIncomeDelta.gameObject.SetActive(true);

        _currentLevelIncomePerMinute.text = "0 $ /min";
        _upgradeCost.text = _business.UpgradeCost.FormatLargeNumber() + " $";
        _nextLevelIncomeDelta.text = "+ " + (_business.NextLevelIncomePerMinute - _business.IncomePerMinute).FormatLargeNumber() + " $ /min";

        _progressView.Set(_business.CurrentLevel, _business.LevelsCount);
    }
}