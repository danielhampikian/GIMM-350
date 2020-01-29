using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 2;
    public Vector3 target;
    public bool shrink;
    void Start()
    {
        GetNewTarget();
        shrink = false;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 1)
        {
            GetNewTarget();
        }
        
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
        if (shrink)
        {
            Shrink();
        }
    }

    public void GetNewTarget()
    {
        target = new Vector3(Random.Range(-5, 5), 1f, Random.Range(-5, 5));
    }
    public void Shrink()
    {
        if(Vector3.Magnitude(transform.localScale) < .1)
        {
            Destroy(gameObject);
        }
        transform.localScale -= new Vector3(.001f, .001f, .001f);
    }
}
