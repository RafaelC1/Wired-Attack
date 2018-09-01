using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {
   
    public List<GameObject> connection_points = new List<GameObject>(2);
    private List<Delivery> traffics = new List<Delivery>();

    public int id = 0;
    public string type = "optical";
    public string wire_name = "fibra";
    public float time_to_travel = 5;
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
        UpdateAllTraffic();
    }

    public void PassReferenceToMachines()
    {
        foreach(GameObject point in connection_points)
        {
            point.GetComponent<Machine>().AddConnection(this.transform.gameObject);
        }
    }

    private void UpdateAllTraffic()
    {
        foreach(Delivery traffic in this.traffics)
        {
            traffic.Update(Time.deltaTime);
        }

        traffics.RemoveAll(traffic => traffic.message_delivered);
    }

    public bool IsConnectedBetween(Machine first_machine, Machine last_machine)
    {
        return (connection_points[0].GetComponent<Machine>().Equals(first_machine) &&
               connection_points[1].GetComponent<Machine>().Equals(last_machine)) ||
               (connection_points[0].GetComponent<Machine>().Equals(last_machine) &&
               connection_points[1].GetComponent<Machine>().Equals(first_machine));
    }

    public void SendBitsBetween(Machine to, Machine from, int bits)
    {
        Delivery new_traffic = new Delivery(to, from, bits, this.time_to_travel, this);
        traffics.Add(new_traffic);
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
    }
}
