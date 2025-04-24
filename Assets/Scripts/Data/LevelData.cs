using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField] private double _incomePerMinute;
    [SerializeField] private double _upgradeCost;
    public double IncomePerMinute => _incomePerMinute;
    public double UpgradeCost => _upgradeCost;
}