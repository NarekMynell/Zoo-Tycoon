using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BusinessBehaviour[] _businesses;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        BusinessBehaviour.OnUpgraded += OnBusinessUpgraded;
        BusinessBehaviour.OnIncomeSent += ReceiveMoneyFromBusiness;
        // BusinessBehaviour.OnLevelSet += CalculateTotalIncome;
    }

    private void OnDisable()
    {
        BusinessBehaviour.OnUpgraded -= OnBusinessUpgraded;
        BusinessBehaviour.OnIncomeSent -= ReceiveMoneyFromBusiness;
        // BusinessBehaviour.OnLevelSet -= CalculateTotalIncome;
    }

    private void Initialize()
    {
        GameData.businesses = _businesses;

        if(GameData.serializableData != null)
        {
            GameData.totalMoney += GameData.serializableData.exitTotalMoney;
            GameData.sound = GameData.serializableData.sound;
            GameData.music = GameData.serializableData.music;

            // Set busineses
            int length = Mathf.Min(_businesses.Length, GameData.serializableData.businesses.Length);
            TimeSpan difference = DateTime.Now - DateTime.Parse(GameData.serializableData.exitDateTime);
            float minutes = (float)difference.TotalMinutes;

            for(int i = 0; i < length; i++)
            {
                BusinessBehaviour business = _businesses[i];
                SerializableData.BusinessData businessData = GameData.serializableData.businesses[i];
                business.Initialize(businessData.level, businessData.accumulatedMoney, minutes);
            }
        }
        else
        {
            foreach(var business in _businesses) business.InitializeAsDefault();
        }

        UpdateTotalIncome();
    }

    private void ReceiveMoneyFromBusiness(double money)
    {
        GameData.totalMoney += money;
    }

    private void OnBusinessUpgraded(double spentMoney)
    {
        GameData.totalMoney -= spentMoney;
        UpdateTotalIncome();
    }

    private void UpdateTotalIncome()
    {
        double totalIncomePerMinute = 0f;
        foreach(var business in _businesses)
        {
            totalIncomePerMinute += business.IncomePerMinute;
        }
        GameData.totalIncomePerMinute = totalIncomePerMinute;
    }
}