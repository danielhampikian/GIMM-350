using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class AutoPlaceItem : MonoBehaviour
{

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    //public GameObject spawnedObject;
    public LayerMask layerMask;
    public bool duplicateObjects;
    public GameObject[] testingGround;
    public Vector3 LastPlacementPosition;
    public float speed;
    public bool isPlacing = false;
    ARRaycastManager m_RaycastManager;
    public ItemPlaceConnector itemPlacedController;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        if (Application.isEditor)
        {
            for (int i = 0; i<testingGround.Length; i++)
            {
                testingGround[i].SetActive(true);
            }
        }
    }
    public void makeObj(Vector3 hitPoint)
    {
        if (this.itemPlacedController != null)
        {
            if (this.itemPlacedController.hasItemBeenPlaced == false)
            {
                isPlacing = true;
                LastPlacementPosition = hitPoint; //if we haven't placed yet, we'll save the position so we can push the item to it instead of midair (if desirable turn this off)
                this.itemPlacedController.getGameObjectToPlace().SetActive(true);
                this.itemPlacedController.getGameObjectToPlace().transform.parent = null;
                /*        if (spawnedObject == null)
                            {
                                spawnedObject = Instantiate(spawnedObject, hitPoint, Quaternion.identity);
                            }*/
                /*            else
                            {*/
                this.itemPlacedController.getGameObjectToPlace().transform.position = Vector3.Lerp(this.itemPlacedController.getGameObjectToPlace().transform.position, hitPoint, Time.deltaTime * speed);
                Vector3 CameraYRot = new Vector3(Camera.main.transform.position.x, hitPoint.y,Camera.main.transform.position.z);
                this.itemPlacedController.getGameObjectToPlace().transform.LookAt(CameraYRot);
                if (!this.itemPlacedController.getGameObjectToPlace().activeSelf)
                {
                    this.itemPlacedController.getGameObjectToPlace().SetActive(true);
                }
                /*            }*/
            }
        }
    }
    void Update()
    {
        if (this.itemPlacedController != null)
        {
            if (Application.isEditor)
            {
                Ray ray = Camera.main.ScreenPointToRay
                    (new Vector3(Camera.main.pixelWidth * .5f, Camera.main.pixelHeight * .5f, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 500f, layerMask))
                {
                    makeObj(hit.point);
                    //this.itemPlacedController.getGameObjectToPlace().transform.rotation = Quaternion.identity;
                    /*if (spawnedObject == null)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hit.point, hit.transform.rotation);
                    }
                    else
                    {
                        spawnedObject.transform.position = hit.point;
                    }*/
                }
            }
            else
            {
                if (m_RaycastManager.Raycast(Camera.main.ViewportPointToRay
                    (new Vector3(.5f, .5f, 0f)), s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    makeObj(hitPose.position);
                    //this.itemPlacedController.getGameObjectToPlace().transform.rotation = hitPose.rotation;
                    /*                if (spawnedObject == null)
                                    {
                                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                                    }
                                    else
                                    {
                                        spawnedObject.transform.position = hitPose.position;
                                    }*/
                }

            }
            if (isPlacing == false && this.itemPlacedController.hasItemBeenPlaced == false)
            {
                HideItem();
            }
            else
            {
                CheckTouchType();
            }
            isPlacing = false;
        }
    }
    public void TapHasOccured()
    {
        if (itemPlacedController.hasItemBeenPlaced == false)
        {
            if (duplicateObjects)
            {
                itemPlacedController.hasItemBeenPlaced = true;

                itemPlacedController.MakeDuplicateToPlace().transform.position = LastPlacementPosition;
            }
            else
            {
                itemPlacedController.hasItemBeenPlaced = true;

                //comment out if you want it to be midair sometimes
                this.itemPlacedController.getGameObjectToPlace().transform.position = LastPlacementPosition;
            }
            }
    }
   public void CheckTouchType()
    {
        if (EventSystem.current.IsPointerOverGameObject() || 
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
                    //tap on actual ground for now
                    TapHasOccured();
                }

            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Debug.Log("Tap has occured");
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    Pose hitPose = s_Hits[0].pose;
                    TapHasOccured();

                }
            }
        
        }
    }
    public void SetNewGameObjectToPlace(ItemPlaceConnector itemPlacedController)
    {
        ShouldWeHideIt();
        //HideItem(this.itemPlacedController.getGameObjectToPlace());
        //spawnedObject = itemPlaced;
        this.itemPlacedController = itemPlacedController;
        //spawnedObject = itemPlacedController.getGameObjectToPlace();
    }
    public void ShouldWeHideIt()
    {
        if (this.itemPlacedController != null)
        {
            if (this.itemPlacedController.hasItemBeenPlaced == false)
            {
                HideItem();
            }
        }
    }
    public void HideItem()
    {
        if (this.itemPlacedController != null)
        {
            this.itemPlacedController.getGameObjectToPlace().SetActive(false);
            this.itemPlacedController.getGameObjectToPlace().transform.parent = Camera.main.transform;
            this.itemPlacedController.getGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }
    public void RemoveItemToPlace()
    {
        this.itemPlacedController = null;
    }
}

