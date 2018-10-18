using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SubjectEventBroadcasting : MonoBehaviour {

    //we need to define the subject that we are interested in changes happening to
    public Button subject;
    //and create an event we can configure in the editor:
    public UnityEvent OnButtonPressed;

	void Start () {
        //Calls the TaskOnClick/TaskWithParameters method when you click the Button
        subject.onClick.AddListener(TaskOnClick); //we add a listener or subscriber directly to the button so that this class is listening and functioning as the subject to another class
        //subject.onClick.AddListener(delegate { TaskWithParameters("Button Pressed calling a delegate function to execute with paramaters"); });
    }


    public void TaskOnClick()
    {
        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the button!");
        OnButtonPressed.Invoke(); //this will notify our event manager class
    }
    /*
    public void TaskWithParameters(string message)
    {
        //Output this to console when the Button is clicked
        Debug.Log(message);
    }
    */
}
