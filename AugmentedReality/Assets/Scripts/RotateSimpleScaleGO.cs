using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
[RequireComponent(typeof(ARRaycastManager))]
public class RotateSimpleScaleGO : MonoBehaviour
{
    public GameObject currentSelected;
    public LayerMask layerMask; //only register raycasts for itemlayer
    public LayerMask planeLayerMask;
    public ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    public AutoPlaceItem autoplaceScript;

    void LateUpdate()
    {
       /* if(autoplaceScript.itemPlacedController != null) //we're placing an item, don't move stuff
        {
            return;
        }*/
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

                if (Physics.Raycast(ray, out hit, 500f, layerMask))
                {
                    currentSelected = hit.collider.gameObject.transform.parent.gameObject;
                    Debug.Log("selected: " + currentSelected.gameObject.name);

                    Ray posRay = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth * .5f, Camera.main.pixelHeight * .5f, 0f));
                    RaycastHit posHit;

                    if (Physics.Raycast(ray, out posHit, 500f, planeLayerMask))
                    {
                        //currentSelected.transform.position = Vector3.Lerp(currentSelected.transform.position, posHit.point, Time.deltaTime * 2f);
                        currentSelected.transform.position = posHit.point;
                    }
                }
            }
            else
            {
                currentSelected = null;
            }
        }
        else
        {
            if(Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit, 500f, layerMask))
                    {
                        currentSelected = hit.collider.gameObject.transform.parent.gameObject;
                    }
                }
                else if(touch.phase == TouchPhase.Moved)
                {
                    if (currentSelected != null)
                    {
                        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                        {
                            //user has selected an object and is moving one finger on the trackable ground plane
                            var hitPose = s_Hits[0].pose;

                            currentSelected.transform.position = hitPose.position;
                        }
                    }
                }
            }
            else if(Input.touchCount == 2) //scale and movement code
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Moved) //when the person is moving do the following
                {
                    if(currentSelected != null)
                    {
                        if(currentSelected.activeSelf == true)//make sure game object is on the screen first
                        {
                            float pinchAmount = 0;
                            Quaternion desiredRotation = currentSelected.transform.rotation;
                            DetectTouchMovement.Calculate();

                            if(Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
                            {
                                //zoom
                                pinchAmount = DetectTouchMovement.pinchDistanceDelta;
                            }
                            if(Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
                            {
                                //rotate
                                Vector3 rotDeg = Vector3.zero;
                                rotDeg.y = DetectTouchMovement.turnAngleDelta;
                                desiredRotation *= Quaternion.Euler(rotDeg);
                            }
                            desiredRotation.x = 0;
                            desiredRotation.z = 0;
                            currentSelected.transform.rotation = desiredRotation;
                            pinchAmount = pinchAmount * .001f;
                            Vector3 newScale = currentSelected.transform.localScale;
                            newScale += pinchAmount * currentSelected.transform.localScale;
                            if(newScale.x > .5 && newScale.x < 2)
                            {
                                currentSelected.transform.localScale = newScale;
                            }
                            //if pinch amoutn is 2, this will be twice as big, if it's .5 it will be half as big
                            //the following for moving:
                            //currentSelected.transform.position += Vector3.forward * pinchAmount;
                        }
                    }
                }
            }
            else
            {
                //deselect item if user isn't touching with one or two fingers
                currentSelected = null;
            }
        }
    }
}
