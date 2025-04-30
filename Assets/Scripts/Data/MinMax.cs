using System;


[Serializable]
public class MinMax
{
    public float min;
    public float max;

    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float GetRandomValue()
    {
        return UnityEngine.Random.Range(min, max);
    }
}