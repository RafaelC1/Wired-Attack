using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : Holdable {
   
    public List<GameObject> connectionsPoints = new List<GameObject>(2);
    public List<Message> messages = new List<Message>();
    public GameController gameController = null;
    
    public string wireType = "optical";
    public float travelTime = 5;
    public Color wireColor;

    void Start()
    {
        GetComponent<LineRenderer>().endColor = wireColor;
        GetComponent<LineRenderer>().startColor = wireColor;
        
        Vector3[] points = new Vector3[] { connectionsPoints[0].transform.position, connectionsPoints[1].transform.position };
        GetComponent<LineRenderer>().SetPositions(points);

        PassReferenceToMachines();
    }

    void Update()
    {
    }

    public void UpdatePointsOnLine()
    {

    }

    public void PassReferenceToMachines()
    {
        foreach(GameObject point in connectionsPoints)
        {
            //point.GetComponent<Machine>().AddConnection(this.transform.gameObject);
        }
    }

    public List<Message> AllMessagesOnWayTo(Machine ownerOfMessage)
    {
        List<Message> messagesOnWay = new List<Message>();
        foreach(Message msg in messages)
        {
            if (msg.to.GetComponent<Machine>() == ownerOfMessage)
                messagesOnWay.Add(msg);
        }
        
        return messagesOnWay;
    }

    public void RemoveMessage(Message messageToDelete)
    {
        messages.Remove(messageToDelete);
        Destroy(messageToDelete.gameObject);
    }

    public bool IsConnectedBetween(GameObject firstMachine, GameObject lastMachine)
    {
        return (connectionsPoints.Contains(firstMachine) &&
                connectionsPoints.Contains(lastMachine));
    }

    public Machine OpositeMachinePointFrom(Machine machine)
    {
        Machine opositePoint = connectionsPoints[0].GetComponent<Machine>();
        if (machine == connectionsPoints[0])
        {
            opositePoint = connectionsPoints[1].GetComponent<Machine>();
        }

        return opositePoint;
    }

    public void OnDestroy()
    {
        foreach(GameObject points in connectionsPoints)
        {
            points.GetComponent<Machine>().RemoveConnection(this.transform.gameObject);
        }

        foreach(Message msg in messages)
        {
            if (msg!= null)
                Destroy(msg.gameObject);
        }
    }
}
