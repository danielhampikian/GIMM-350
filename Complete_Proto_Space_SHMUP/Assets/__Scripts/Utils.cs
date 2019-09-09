using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

	// Materials functions:

    //returns list of all materials for a game object param - static so we can access it anywhere
    static public Material[]GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();
        List<Material> mats = new List<Material>(); //we need an array of undetermined length, so a list will do to hold them
        foreach(Renderer rend in rends) //gets each renderer and grabs all the materials in that component
        {
            mats.Add(rend.material);
        }
        return (mats.ToArray()); //convert the list to an array
    }
}

/* Useful stuff:
 * 
 *  cube = transform.Find("Cube").gameObject; //find a reference by transform
 *  bndCheck = GetComponent<BoundsCheck>(); // then get a script attached to that object
 *  Vector3 vel = Random.onUnitSphere; //Random xyz velocity
 *  vel.Normalize(); //make length of vector 1m
 *   vel *= Random.Range(driftMinMax.x, driftMinMax.y, drifMinMax.z); //set the drift
 *           transform.rotation = Quaternion.identity;
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), 
            Random.Range(rotMinMax.x, rotMinMax.y), 
            Random.Range(rotMinMax.x, rotMinMax.y));
 *   
 */
