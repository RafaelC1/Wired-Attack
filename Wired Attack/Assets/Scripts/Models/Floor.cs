using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Dynamic;

public class Floor : MonoBehaviour
{
    public int id = 0;
    public GameObject controller = null;
    public GameObject floorHolder = null;
    public GameObject objectHolded = null;

    public GameObject background = null;

    public MapController.TypeOfFloor type = MapController.TypeOfFloor.GRASS;
    public MapController.SideOfFloor side = MapController.SideOfFloor.TOP_LEFT;

    void Start()
    {
        ActiveBackGround(false);
        PositionateObjectOnCenter();
    }

    void Update()
    {

    }
    
    public void ReceiveObjectToHold(GameObject objectToHold)
    {
        objectHolded = objectToHold;
        objectHolded.GetComponent<Holdable>().currentFloor = this;
        PositionateObjectOnCenter();
    }

    public void RemoveHoldedObject()
    {
        Destroy(objectHolded);
        objectHolded = null;
    }

    public void PositionateObjectOnCenter()
    {
        if(IsHoldingSomething())
        {
            objectHolded.transform.position = this.floorHolder.transform.position;
        }        
    }

    public bool IsHoldingSomething()
    {
        return objectHolded != null;
    }

    public void ActiveBackGround(bool activate)
    {
        background.GetComponent<SpriteRenderer>().enabled = activate;
    }

    public void TurnToEdit(bool turn)
    {
        GetComponent<BoxCollider2D>().enabled = turn;
    }

    public void OnDestroy()
    {
        Destroy(objectHolded);
    }
}
