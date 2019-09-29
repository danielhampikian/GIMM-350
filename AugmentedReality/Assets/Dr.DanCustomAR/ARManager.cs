using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ARManager : MonoBehaviour
{
    public bool buttonPressed = false;
    public string objName = "";
    public void PlaceObject(string name)
    {

        Debug.Log("placing object" + name);
        objName = name;
        buttonPressed = true;
    }
    
}
