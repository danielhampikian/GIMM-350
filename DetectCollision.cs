using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider anything)
    {
        if(anything.gameObject.name == "Enemy")
        {
            anything.gameObject.GetComponent<AIMovement>().shrink = true;
        }
    }
}
