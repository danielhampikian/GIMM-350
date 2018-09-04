using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incomplete_Utilitities : MonoBehaviour {

    //TODO: Write a public variable that enables you to instantiate a prefab
    //TODO: Write a public variable that enables you to access the Instantiate script by linking in the editor
    public float spacing = 2f;

    public void instantiatePrefab()
    {
        //TODO: Instantiate the linked prefab at the origin (0,0,0)
    }
    public void instantiatePrefabAtPosition(Vector3 pos)
    {
        //TODO: Instantiate prefab a a position that is passed in as a parameter
    }
    public void instantiatPrefabsEqualSpacing()
    {
        //TODO: Get the value input by user by accessing a public method of the Instantiate Script
        int i = 0;
        int r = 0;
        //TODO: assign initial r.
        Vector3 pos;
        while (i < r)
        {
            pos = new Vector3(i * spacing, 2, i * spacing);
            instantiatePrefabAtPosition(pos);
            //TODO: Write two more loops inside this while loop to make a three dimensional cube with equal spacing between spheres
            i++;
        }

    }
}
