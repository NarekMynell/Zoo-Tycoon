using System;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public static class DoubleExtensions
{
    public static string FormatLargeNumber(this double value, bool roundOnlyAfterReaching = false)
    {
        const double QUADRILLION = 1_000_000_000_000_000;
        const double TRILLION = 1_000_000_000_000;
        const double BILLION = 1_000_000_000;
        const double MILLION = 1_000_000;
        const double THOUSAND = 1_000;

        if (value >= QUADRILLION)
        {
            double quadrillion = value / QUADRILLION;
            return roundOnlyAfterReaching ? 
                $"{Math.Floor(quadrillion * 10) / 10}q" : 
                $"{Math.Round(quadrillion * 10) / 10}q";
        }
        else if (value >= TRILLION)
        {
            double trillions = value / TRILLION;
            return roundOnlyAfterReaching ? 
                $"{Math.Floor(trillions * 10) / 10}t" : 
                $"{Math.Round(trillions * 10) / 10}t";
        }
        else if (value >= BILLION)
        {
            double billions = value / BILLION;
            return roundOnlyAfterReaching ? 
                $"{Math.Floor(billions * 10) / 10}b" : 
                $"{Math.Round(billions * 10) / 10}b";
        }
        else if (value >= MILLION)
        {
            double millions = value / MILLION;
            return roundOnlyAfterReaching ? 
                $"{Math.Floor(millions * 10) / 10}m" : 
                $"{Math.Round(millions * 10) / 10}m";
        }
        else if (value >= THOUSAND)
        {
            double thousands = value / THOUSAND;
            return roundOnlyAfterReaching ? 
                $"{Math.Floor(thousands * 10) / 10}k" : 
                $"{Math.Round(thousands * 10) / 10}k";
        }
        else
        {
            return value.ToString("F0");
        }
    }

    private static NumberFormatInfo _comaFormatInfo = new NumberFormatInfo
    {
        NumberGroupSeparator = ",",
        NumberGroupSizes = new[] { 3 },
        NumberDecimalDigits = 0
    };
    
    public static string GetComaFormat(this double num)
    {

        // Convert to string with the custom format
        string formattedNumber = num.ToString("N0", _comaFormatInfo);
        return formattedNumber;
    }
}