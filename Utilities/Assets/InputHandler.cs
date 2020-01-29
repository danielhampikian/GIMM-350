using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class InputHandler
{
    public static float totalGrade;
    public static double processText(string inWQeight, string inGrades)
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
    public static double[] parseAsArray(string doubleString){
        string[] numAsString = doubleString.Split(' ');
        double[] arRet = new double[numAsString.Length];

        int i  = 0;
        foreach (var num in numAsString)
        {
            i++;
            arRet[i] = getNum(numAsString[i]);
        }       
        //PlayerPrefs.SetFloat("total", totalGrade);
        return arRet;        
}
    public static double findWeightedAverage(double[] numbersToAverage, double weight)
    {
        double total=0;
        foreach(double num in numbersToAverage)
        {
            total += num;
        }
        return (total / numbersToAverage.Length) * weight;
    }
    public static double getNum(string inS)
    {
        try
        {
            double num = Double.Parse(inS);
            return num;
        }
        catch (FormatException e)
        {
            Debug.Log(e);
            return -1;
        }
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
        double num;
        if (Double.TryParse(inS, out double j))
        {
            num = j;
            return true;
        }
        else
            return false;

    }
}
