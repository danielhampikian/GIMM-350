using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public float rotationsPerScecond = 0.1f;
    public int levelShown = 0;

    Material mat;

	void Start () {
        mat = GetComponent<Renderer>().material; //how to grab a reference to a material
	}
	
	
	void Update () {
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel); //we can access the hero safely because he is a static singleton
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        //here's how we rotation the sheild per update:
        float rZ = -(rotationsPerScecond * Time.time * 36000) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
