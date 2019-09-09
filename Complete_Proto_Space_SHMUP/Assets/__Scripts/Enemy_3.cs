using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy {

    public float lifeTime = 5;
    public Vector3[] points;
    public float birthTime;

    void Start()
    {
        points = new Vector3[3];
        points[0] = pos;//start position set by main.spawnenemy()
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;
        Vector3 v;
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;

        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        birthTime = Time.time;
    }
    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        Vector3 p01, p12;
        //implement Bezier curve, interpolation between more than two points

        //easing:
        u = u - 0.1f * (Mathf.Sin(u * Mathf.PI * 2));

        p01 = (1 - u) * points[0] + u * points[1]; //interpolate between points
        p12 = (1 - u) * points[1] + u * points[2]; //interpolate between points
        pos = (1 - u) * p01 + u * p12; //interpolate between 4 points
        base.Move();
    }
}
