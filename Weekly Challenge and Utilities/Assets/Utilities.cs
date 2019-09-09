using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utilities : MonoBehaviour {

    public GameObject prefab;
    public Instantiate instScript;
    public float spacing = 2f;

    public void instantiatePrefab()
    {
        Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
    public void instantiatePrefabAtPosition(Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity);
    }
    public void instantiatPrefabsEqualSpacing()
    {
        int i = 0;
        int r = instScript.r; 
        Vector3 pos;
        while (i < r) //i=0, r = whatever is input
        {
            pos = new Vector3(i * spacing, 2, i * spacing);
            instantiatePrefabAtPosition(pos);
            for (int j = 0; j < r; j++)
            {
                pos = new Vector3(i * spacing, 2 + j * spacing, i * spacing);
                instantiatePrefabAtPosition(pos);
                
                for (int k = 0; k < r; k++)
                {
                    pos = new Vector3(i * spacing + k * spacing, 2 + j * spacing, i * spacing);
                    instantiatePrefabAtPosition(pos);
                }
                
            }
            
            i++;
        }
    }
}
