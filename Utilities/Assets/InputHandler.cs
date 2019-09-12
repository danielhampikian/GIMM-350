using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class InputHandler
{

    public static string processText(string inS)
    {
       return inS;
    }
    public static int getAverageChars(string inS)
    {
        string[] words = inS.Split(' ');
        int sum = 0;
        foreach (var word in words)
        {
            sum += word.Length;
        }
        return sum / words.Length;
    }
    public static bool isNum(string inS)
    {
        try
        {
            double num = Double.Parse(inS);
            return true;
        }
        catch (FormatException e)
        {
            Debug.Log(e);
            return false;
        }
    }
    public static bool betterIsNum(string inS)
    {
        if (Double.TryParse(inS, out double j))
            return true;
        else
            return false;

    }
}
