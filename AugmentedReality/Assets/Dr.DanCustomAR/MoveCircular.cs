using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCircular : MonoBehaviour
{
    public Transform center;
    public float heightOffset;
    public float speed;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(center.position, Vector3.down, speed * Time.deltaTime);
    }

}
