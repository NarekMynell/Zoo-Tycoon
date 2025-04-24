using UnityEngine;

public class BusinessBehaviour : MonoBehaviour, IBusiness, IClickable
{
    [SerializeField] private Scriptables.BusinessData _businessData;
    [SerializeField] private LevelVisualSeter _levelSeter;

    public static event System.Action<BusinessBehaviour> OnBusinessClicked;
    public static event System.Action<double> OnIncomeSent;
    public static event System.Action<double> OnUpgraded;
    // public static event System.Action OnLevelSet;

    public int CurrentLevel {get; private set;} = 0;
    public double AccumulatedMoney {get; private set;}
    public string Name => _businessData.Name;
    public int LevelsCount => _businessData.LevelsCount;
    public bool HasUpgrade => CurrentLevel < LevelsCount;
    public double IncomePerMinute => CurrentLevel > 0 ? _businessData.GetLevelData(CurrentLevel).IncomePerMinute : 0;
    public double UpgradeCost => HasUpgrade ? _businessData.GetLevelData(CurrentLevel + 1).UpgradeCost : 0d;
    public double NextLevelIncomePerMinute => HasUpgrade ? _businessData.GetLevelData(CurrentLevel + 1).IncomePerMinute : 0d;

    private float _lastSendTime = 0;
    private bool _isInitialized = false;


    private void Start()
    {
        if(!_isInitialized)
        {
            Initialize(_businessData.DefaultLevel, AccumulatedMoney, 0);
        }
        if(_businessData.AutoSendMoney) SendIncome();
    }

    private void Update()
    {
        AccumulatedMoney += IncomePerMinute * Time.deltaTime / 60f;

        if(_businessData.AutoSendMoney)
        {
            if(Time.time - _lastSendTime >= 1f)
            {
                SendIncome();
                _lastSendTime = Time.time;
            }
        }
    }

    public void InitializeAsDefault()
    {
        Initialize(_businessData.DefaultLevel, AccumulatedMoney, 0);
    }

    public void Initialize(int level, double lastAccumulatedMoney, float deltaMinutes)
    {
        CurrentLevel = level;
        _levelSeter.Init(CurrentLevel);
        AccumulatedMoney = lastAccumulatedMoney + IncomePerMinute * deltaMinutes;
        _isInitialized = true;
    }

    public void SendIncome()
    {
        OnIncomeSent?.Invoke(AccumulatedMoney);
        AccumulatedMoney = 0d;
    }

    public void UpgradeLevel()
    {
        if(!HasUpgrade) return;
        
        double spentMoney = UpgradeCost;
        CurrentLevel++;
        _levelSeter.SetLevel(CurrentLevel);
        OnUpgraded?.Invoke(spentMoney);
    }

    public void OnClicked()
    {
        OnBusinessClicked?.Invoke(this);
    }
}