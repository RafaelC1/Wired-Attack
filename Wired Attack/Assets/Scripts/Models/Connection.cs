using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour {
   
    public List<GameObject> connection_points = new List<GameObject>(2);
    public List<Message> messages = new List<Message>();
    public GameController game_controller = null;

    public int id = 0;
    public string wire_type = "optical";
    public string connection_name = "fibra";
    public float travel_time = 5;
    public Color wire_color;

    void Start()
    {
        GetComponent<LineRenderer>().endColor = wire_color;
        GetComponent<LineRenderer>().startColor = wire_color;

        Vector3[] points = new Vector3[] { connection_points[0].transform.position, connection_points[1].transform.position };
        GetComponent<LineRenderer>().SetPositions(points);

        PassReferenceToMachines();
    }

    void Update()
    {
    }

    public void PassReferenceToMachines()
    {
        foreach(GameObject point in connection_points)
        {
            //point.GetComponent<Machine>().AddConnection(this.transform.gameObject);
        }
    }

    public List<Message> AllMessagesOnWayTo(Machine owner_of_message)
    {
        List<Message> messages_on_way = new List<Message>();
        foreach(Message msg in messages)
        {
            if (msg.to.GetComponent<Machine>() == owner_of_message)
                messages_on_way.Add(msg);
        }
        
        return messages_on_way;
    }

    public void RemoveMessage(Message message_to_delete)
    {
        messages.Remove(message_to_delete);
        Destroy(message_to_delete.gameObject);
    }

    public bool IsConnectedBetween(GameObject first_machine, GameObject last_machine)
    {
        return (connection_points.Contains(first_machine) &&
                connection_points.Contains(last_machine));
    }

    public Machine OpositeMachinePointFrom(Machine machine)
    {
        Machine oposite_point = connection_points[0].GetComponent<Machine>();
        if (machine == connection_points[0])
        {
            oposite_point = connection_points[1].GetComponent<Machine>();
        }

        return oposite_point;
    }

    public void OnDestroy()
    {
        foreach(GameObject points in connection_points)
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
