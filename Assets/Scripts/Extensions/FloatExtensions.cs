using System;
using UnityEngine;

public static class FloatExtensions
{
    public static float[] GetRandomParts(this float total, int partsCount)
    {
        if (partsCount <= 0)
            throw new ArgumentException("partsCount must be greater than 0");

        float[] raw = new float[partsCount];
        float sum = 0f;

        for (int i = 0; i < partsCount; i++)
        {
            raw[i] = UnityEngine.Random.value; // [0..1)
            sum += raw[i];
        }

        float[] result = new float[partsCount];
        for (int i = 0; i < partsCount; i++)
        {
            result[i] = raw[i] / sum * total;
        }

        return result;
    }
}
