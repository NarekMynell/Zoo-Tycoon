using System;


[Serializable]
public class SerializableData
{
    public BusinessData[] businesses;
    public string exitDateTime;
    public double exitTotalMoney;
    public bool sound;
    public bool music;

    public SerializableData(int businessCount)
    {
        businesses = new BusinessData[businessCount];
    }


    [Serializable]
    public struct BusinessData
    {
        public double accumulatedMoney;
        public int level;
    }
}