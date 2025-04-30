using System;
using System.Collections.Generic;

public static class ListExtensions
{
    private static Random rng = new Random();

    public static List<T> GetRandomItems<T>(this List<T> list, int count)
    {
        if (count < 0)
            throw new ArgumentException("Count must be non-negative.");
        if (count > list.Count)
            throw new ArgumentException("Count cannot be greater than the list size.");

        // Կրկնօրինակում ենք list-ը, որպեսզի չփոխենք օրիգինալը
        List<T> copy = new List<T>(list);

        // Fisher-Yates shuffle մինչև count տարր
        for (int i = 0; i < count; i++)
        {
            int j = rng.Next(i, copy.Count);
            (copy[i], copy[j]) = (copy[j], copy[i]);
        }

        return copy.GetRange(0, count);
    }
}
