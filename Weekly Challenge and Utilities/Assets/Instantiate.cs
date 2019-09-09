using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instantiate : MonoBehaviour {

    GameObject sphere;
    GameObject[] spheres;
    Vector3[] positions;
    public InputField input;
    public int r;
    private int i, j;
	// Use this for initialization
	void Start ()
    {
        input.onValueChanged.AddListener(delegate { getInput(); });
    }
    private void Update()
    {
        
    }

    private void makeDiamond()
    {
        int xOffset = 5;
        int yOffset = 14;
        int pos = 0;
        positions = new Vector3[r * 100];
        for (i = 0; i <= r; i++) //top half this executes until r but we want to fill out the daimond row by row in a second loop that spaces our objects accordingly
        {
            for (j = 1; j <= 2 * i - 1; j++) //for example, when i is 3, j increments to 5 and we subtract 3 for the x position giving us a position range from -2 to 2 for x. 
            {
                sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                int xPos = j + xOffset;
                sphere.transform.position = new Vector3(xPos, yOffset, 0);
                sphere.AddComponent<Rigidbody>();
                positions[pos] = sphere.transform.position;
                pos++;
                sphere.tag = "s";
                Debug.Log("In j loop inside i loop with i = " + i + " with yOffset = " + yOffset + " xOffset = " + xOffset + " j = " + j + " final x position = " + xPos);
            }
            yOffset -= 2;
            xOffset -= 1;
            
        }
        
        xOffset += 2;

        for (i = r - 1; i >= 1; i--) //bottom half
        {

            for (j = 1; j <= 2 * i - 1; j++)
            {
                sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = new Vector3(j + xOffset, yOffset, 0);
                sphere.tag = "s";
                positions[pos] = sphere.transform.position;
                pos++;
                sphere.AddComponent<Rigidbody>();
            }
            yOffset -= 2;
            xOffset += 1;
        }
        
    }
    public int getInput()
    {
        if(spheres!= null)
        {
            spheres = null;
        }
        spheres = GameObject.FindGameObjectsWithTag("s");
        if (spheres != null)
        {
            foreach (GameObject s in spheres)
            {
                if(s != null)
                Destroy(s);
            }
        }
        int num;
        if(int.TryParse(input.text, out num))
        {
            if (num < 10)
                r = num;
            else
                Debug.Log("Enter a number less than ten");
        }
        else {
            r = 0;
        }
        makeDiamond();
        return r;
    }
    public void returnToPositions()
    {
        spheres = GameObject.FindGameObjectsWithTag("s");
        int pos = 0;
        if (spheres != null)
        {
            foreach (GameObject s in spheres)
            {
                if (s != null)
                {
                    s.transform.position = positions[pos];
                    pos++;
                }
            }
        }
    }
}


 

