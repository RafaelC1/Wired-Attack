using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSerialized : Serializable
{
    public int teamId;
    public string machineModel = "";

	public MachineSerialized(Machine machineToSerialize)
    {
        id = machineToSerialize.id;
        teamId = machineToSerialize.team.GetHashCode();
        floorId = machineToSerialize.currentFloor.GetComponent<Floor>().id;
        machineModel = machineToSerialize.model;
    }
}
