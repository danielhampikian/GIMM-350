using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Cube")
        {
            foreach (var contact in collision.contacts)
            {
                MakeSpheresOnCollisionPoints(contact.point);
            }
        }
    }
    private void MakeSpheresOnCollisionPoints(Vector3 colPoint)
    {
        Transform smallSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        smallSphere.position = colPoint;
        smallSphere.localScale = Vector3.one * .1f;
        smallSphere.GetComponent<Renderer>().material.color = Color.red;
    }
}
