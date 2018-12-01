using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionSerialized : Serializable {
    
    public string wireType = "";
    public int[] connectionIds = new int[2];

	public ConnectionSerialized(Connection connection_to_serialize)
    {
        id = connection_to_serialize.id;
        wireType = connection_to_serialize.wireType;
        connectionIds[0] = connection_to_serialize.connectionsPoints[0].transform.GetComponent<Machine>().id;
        connectionIds[1] = connection_to_serialize.connectionsPoints[1].transform.GetComponent<Machine>().id;
    }
}
