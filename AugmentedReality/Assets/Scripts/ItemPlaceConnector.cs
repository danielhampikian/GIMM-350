using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlaceConnector : MonoBehaviour
{
    public bool hasItemBeenPlaced = false;
    public GameObject ItemToSetIntoPlacer;
    public AutoPlaceItem PlacerScript;
    void Start()
    {
        if (hasItemBeenPlaced == false)
        {
            ItemToSetIntoPlacer.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonClicked()
    {
        if (hasItemBeenPlaced == false)
        {
            if (PlacerScript.itemPlacedController != this)
            {
                PlacerScript.SetNewGameObjectToPlace(this);
            }
            else
            {
                PutItemAway();
            }
        }
        else
        {
            PutItemAway();
        }
    }
    public void PutItemAway()
    {
        PlacerScript.SetNewGameObjectToPlace(this);
        hasItemBeenPlaced = false;
        PlacerScript.HideItem();
        PlacerScript.RemoveItemToPlace();
    }
    public GameObject MakeDuplicateToPlace()
    {
        GameObject duplicateObj = Instantiate(ItemToSetIntoPlacer, ItemToSetIntoPlacer.transform.position, ItemToSetIntoPlacer.transform.rotation, ItemToSetIntoPlacer.transform.parent);
        ButtonClicked();
        return duplicateObj;
    }
    public GameObject getGameObjectToPlace()
    {
        return ItemToSetIntoPlacer;
    }
}
