using System;
using UnityEngine;

[ExecuteAlways]
public class AnimalCage : MonoBehaviour, IBusiness, ILevelable
{
    [SerializeField] private Scriptables.BusinessData _businessData;
    [SerializeField] private AnimalCageLevelSeter _levelSeter;
    [SerializeField] private int _startLevel = 0;


    public float GetIncomeInMinute()
    {
        return _businessData.GetLevelIncome(GetLevel());
    }

    public int GetLevel()
    {
        throw new System.NotImplementedException();
    }

    public float SendIncome()
    {
        throw new System.NotImplementedException();
    }

    public void SetLevel(int level)
    {
        _levelSeter.SetLevel(level);
    }

    public void UpgradeLevel(int level)
    {
        throw new System.NotImplementedException();
    }
}
