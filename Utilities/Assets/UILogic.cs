using UnityEngine;
using UnityEngine.UI;
using System;
public class UILogic : MonoBehaviour
{
    public Text text;
    public string processedText;
    public InputField weightIn;
    public InputField gradesIn;


    public void getInput()
     
    {
        processText(weightIn.textComponent.text, gradesIn.textComponent.text);
    }
    public void displayOutput()
    {
        Debug.Log(processedText);
        string typeOfInput = "none";
        if (InputHandler.betterIsNum(processedText))
        {
            typeOfInput = "number";
        }
        else
        {
            typeOfInput = "string";
        }
        text.text = typeOfInput;
        
        //text.text = InputHandler.getAverageChars(processedText).ToString();
    }
    public void processText(string inS)
    {
        Debug.Log("Before input handler: " + inS);
        //this.processedText = inS;
        this.processedText = InputHandler.processText(inS);
        displayOutput();
    }

}
