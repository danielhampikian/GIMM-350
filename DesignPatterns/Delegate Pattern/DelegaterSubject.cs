using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelegaterSubject : MonoBehaviour {

	public delegate void MethodToDefine();
    public static event MethodToDefine methodDefinedElsewhere;

	void Start () {
		
	}
    public void buttonClicked()
    {
        methodDefinedElsewhere();
    }
	
}
