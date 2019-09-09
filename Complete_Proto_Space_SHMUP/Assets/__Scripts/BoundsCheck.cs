using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour {

    public float radius = 1f;
    public float camWidth;
    public float camHeight;
    public bool keepOnScreen = true;
    public bool isOnScreen = true;


	void Awake () {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
	}

    // Update is called once per frame
    void LateUpdate () {
        Vector3 pos = transform.position;
        isOnScreen = true;
        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            isOnScreen = false;
        }
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            isOnScreen = false;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            isOnScreen = false;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            isOnScreen = false;
        }
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
        }
    }

}
