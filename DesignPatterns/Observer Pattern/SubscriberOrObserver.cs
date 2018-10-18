using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubscriberOrObserver : MonoBehaviour {

    public SubjectEventBroadcasting subjectToListenTo;
    public Text scoreUpdateTextbox;
    public int score;

    //we're listening for button pressed to update various things here:
	public void ButtonPressed()
    {
        Debug.Log("Button Pressed update score");
        score++;
        scoreUpdateTextbox.text = "Score: " + score;
    }
	void Start () {
        score = 0;
        //here we add the listener or observer for the subject's button press event:
        subjectToListenTo.OnButtonPressed.AddListener(ButtonPressed);
    }

}
