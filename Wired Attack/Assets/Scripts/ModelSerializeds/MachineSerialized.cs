using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSerialized : Serializable
{
    public int team_id;
    public string machine_model = "";

	public MachineSerialized(Machine machine_to_serialize)
    {
        id = machine_to_serialize.id;
        team_id = machine_to_serialize.team.GetHashCode();
        floor_id = machine_to_serialize.current_floor.GetComponent<Floor>().id;
        machine_model = machine_to_serialize.model;
    }
}
