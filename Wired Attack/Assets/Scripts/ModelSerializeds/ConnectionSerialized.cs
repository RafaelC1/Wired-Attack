using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionSerialized : Serializable {
    
    public string wire_type = "";
    public int[] connection_ids = new int[2];

	public ConnectionSerialized(Connection connection_to_serialize)
    {
        id = connection_to_serialize.id;
        wire_type = connection_to_serialize.wire_type;
        connection_ids[0] = connection_to_serialize.connection_points[0].transform.GetComponent<Machine>().id;
        connection_ids[1] = connection_to_serialize.connection_points[1].transform.GetComponent<Machine>().id;
    }
}
