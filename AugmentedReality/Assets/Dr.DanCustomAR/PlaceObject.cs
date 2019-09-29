using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObject : MonoBehaviour
{
    public GameObject dragon;
    public GameObject cowgirl;
    public GameObject projectile;
    GameObject goProj;
    public GameObject testFloor;
    public bool firing;
    public bool fire;
    public LayerMask layerM;
    public Vector3 lastPlacedPosition;
    public float placeSpeed = 3f;
    public ARManager arManager;
    ARRaycastManager arRaycastManager;

    static List<ARRaycastHit> arHits = new List<ARRaycastHit>();
    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        if (Application.isEditor)
        {
            testFloor.SetActive(true);
        }
    }
    void placeObject(Vector3 pos)
    {
        if (arManager.buttonPressed == true && arManager.objName != "projectile")
        {
            lastPlacedPosition = pos;
            GameObject go = new GameObject();
            if (arManager.objName == "cowgirl")
            {
               go  = Instantiate(cowgirl, transform);

            }
            if(arManager.objName == "dragon")
            {
                go = Instantiate(dragon, transform);
            }

            //isPlacing = true;
            go.transform.parent = null;
            go.transform.position = pos;
        }

            else if (arManager.buttonPressed == true && arManager.objName == "projectile")
            {
                fire = true;
            }
            arManager.buttonPressed = false;

        
    }

    void Update()
    {
        Vector3 pos = new Vector3();
        if (Application.isEditor)
        {
            Ray ray = Camera.main.ScreenPointToRay
                (new Vector3(Camera.main.pixelWidth * .5f, Camera.main.pixelHeight * .5f, 0f));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 500f, layerM))
            {
                pos = hit.point;
                placeObject(pos);
            }
        }
        else
        {
            if(arRaycastManager.Raycast(Camera.main.ViewportPointToRay
                (new Vector3(.5f,.5f,0f)), arHits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = arHits[0].pose;
                pos = hitPose.position;
                placeObject(pos);
            }
        }

        if (fire)
        {
            goProj = Instantiate(projectile, transform);

            //isPlacing = true;
            goProj.transform.parent = null;
            firing = true;
            fire = false;
        }
        if (firing)
        {
            goProj.transform.position = Vector3.Lerp(goProj.transform.position, pos, 2f * Time.deltaTime);
            if (Vector3.Distance(goProj.transform.position,pos) < .1)
            {
                firing = false;
            }
        

        }
    }
}
