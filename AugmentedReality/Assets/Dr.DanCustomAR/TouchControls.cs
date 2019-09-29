using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
[RequireComponent(typeof(ARRaycastManager))]

public class TouchControls : MonoBehaviour
{
    public GameObject currentObj;
    public LayerMask layerGround;
    public LayerMask layerObj;
    public ARRaycastManager raycastManager;
    static List<ARRaycastHit> rHits = new List<ARRaycastHit>();
    
    
    void LateUpdate()
    {
        if(EventSystem.current.IsPointerOverGameObject() ||
            EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }
        if (Application.isEditor)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 500f, layerObj))
                {
                    currentObj = hit.collider.gameObject;
                    Debug.Log("selected is " + currentObj.gameObject.name);

                    Ray posRay = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth * .5f, Camera.main.pixelHeight * .5f, 0f));
                    RaycastHit posHit;

                    if (Physics.Raycast(ray, out posHit, 500f, layerGround))
                    {
                        currentObj.transform.position = posHit.point;
                    }
                }
            }
            else
            {
                currentObj = null;
            }
        }
        //we're using a touchscreen
        else
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 500f, layerObj))
                    {
                        currentObj = hit.collider.gameObject;
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (currentObj != null)
                    {
                        if (raycastManager.Raycast(touch.position, rHits, TrackableType.PlaneWithinPolygon))
                        {
                            var hitPose = rHits[0].pose;
                            currentObj.transform.position = hitPose.position;
                        }
                    }
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Moved)
                {
                    if(currentObj != null)
                    {
                        float pinch = 0;
                        Quaternion desiredRotation = currentObj.transform.rotation;
                        DetectTouchMovement.Calculate();
                        if(Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
                        {
                            pinch = DetectTouchMovement.pinchDistanceDelta;

                        }
                        if(Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
                        {
                            Vector3 rotDeg = Vector3.zero;
                            rotDeg.y = DetectTouchMovement.turnAngleDelta;
                            desiredRotation *= Quaternion.Euler(rotDeg);
                        }
                        currentObj.transform.rotation = desiredRotation;
                        pinch = pinch * .005f;
                        Vector3 pinchScale = currentObj.transform.localScale;
                        pinchScale += pinch * currentObj.transform.localScale;
                        if (pinchScale.x > .5 && pinchScale.x < 2)
                        {
                        }
                        currentObj.transform.localScale = pinchScale;

                    }
                }
            }

            else
            {
                currentObj = null;
            }
        }
    }
}
