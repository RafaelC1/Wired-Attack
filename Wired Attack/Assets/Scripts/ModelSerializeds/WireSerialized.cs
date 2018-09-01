using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSerialized {

    public int id = 0;
    public string wire_type = "";
    public int[] connection_ids = new int[2];

	public WireSerialized(Wire wire_to_serialize)
    {
        id = wire_to_serialize.id;
        wire_type = wire_to_serialize.type;
        connection_ids[0] = wire_to_serialize.connection_points[0].transform.GetComponent<Machine>().id;
        connection_ids[1] = wire_to_serialize.connection_points[1].transform.GetComponent<Machine>().id;
    }
}
