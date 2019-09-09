using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingGameObject : MonoBehaviour {

    //make another button where the planet rises and does something else...
    Animator anim;
	void Start () {
        anim = GetComponent<Animator>();
        DelegaterSubject.methodDefinedElsewhere += makePlanetRise;
        DelegaterSubject.methodDefinedElsewhere += logAnimationHappened;
	}
	
    public void makePlanetRise()
    {
        anim.SetBool("isRising", true);
        DelegaterSubject.methodDefinedElsewhere -= makePlanetRise;
        DelegaterSubject.methodDefinedElsewhere += stopPlanetRise;
    }
    public void stopPlanetRise()
    {
        anim.SetBool("isRising", false);
        DelegaterSubject.methodDefinedElsewhere -= stopPlanetRise;
        DelegaterSubject.methodDefinedElsewhere += makePlanetRise;
    }
    public void logAnimationHappened()
    {
        Debug.Log("Hey, it worked");
    }
	
	void Update () {
		
	}
}
