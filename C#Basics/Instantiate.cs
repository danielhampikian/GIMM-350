using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instantiate : MonoBehaviour {

    GameObject sphere;
    GameObject[] spheres;
    public InputField input;
    public int r;
    private int i, j;
	// Use this for initialization
	void Start ()
    {
        input.onValueChanged.AddListener(delegate { getInput(); });
    }

    private void makeDiamond()
    {
        int xOffset = 0;
        int yOffset = 10;
        spheres = new GameObject[r * 100];
        for (i = 0; i <= r; i++) //top half this executes until r but we want to fill out the daimond row by row in a second loop that spaces our objects accordingly
        {
            for (j = 1; j <= 2 * i - 1; j++) //for example, when i is 3, j increments to 5 and we subtract 3 for the x position giving us a position range from -2 to 2 for x. 
            {
                sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                int xPos = j + xOffset;
                sphere.transform.position = new Vector3(xPos, yOffset, 0);
                sphere.tag = "s";
                Debug.Log("In j loop inside i loop with i = " + i + " with yOffset = " + yOffset + " xOffset = " + xOffset + " j = " + j + " final x position = " + xPos);
            }
            yOffset -= 2;
            xOffset -= 1;
            
        }

        //Weekly challenge: Finish the bottom half of the daimond below:
        //Bonus: Correct the input format exception when backspace is hit in the input feild.
    }
    public void getInput()
    {
        spheres = GameObject.FindGameObjectsWithTag("s");
        if (spheres != null)
        {
            foreach (GameObject s in spheres)
            {
                if(s != null)
                Destroy(s);
            }
        }
     
        r = int.Parse(input.text);
        makeDiamond();
    }
}


 

