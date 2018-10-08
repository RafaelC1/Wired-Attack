using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializable {

    public int id = 0;
    public int floor_id = 0;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
