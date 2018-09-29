using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Dynamic;

public class Floor : MonoBehaviour
{
    public int id = 0;
    public GameObject controller = null;
    public GameObject floor_holder = null;
    public GameObject object_holded = null;

    public GameObject background = null;

    public MapController.TypeOfFloor type = MapController.TypeOfFloor.grass;
    public MapController.SideOfFloor side = MapController.SideOfFloor.top_left;

    void Start()
    {
        ActiveBackGround(false);
        PositionateObjectOnCenter();
    }

    void Update()
    {

    }
    
    public void ReceiveObjectToHold(GameObject object_to_hold)
    {
        object_holded = object_to_hold;
        object_holded.GetComponent<Machine>().current_floor = this;
        PositionateObjectOnCenter();
    }

    public void RemoveHoldedObject()
    {
        Destroy(object_holded);
        object_holded = null;
    }

    public void PositionateObjectOnCenter()
    {
        if(IsHoldingSomething())
        {
            object_holded.transform.position = this.floor_holder.transform.position;
        }        
    }

    public bool IsHoldingSomething()
    {
        return object_holded != null;
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
        Destroy(object_holded);
    }
}
