using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Debug.Log("move forward: " + moveForward);
        float moveSideways = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Debug.Log("move sideways: " + moveSideways);
        Vector3 pos = new Vector3(moveSideways, 0f, moveForward);
        transform.position += pos;
    }
}
