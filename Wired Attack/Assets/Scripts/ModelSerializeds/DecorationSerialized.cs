using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSerialized : Serializable {

    public Decoration.TypeOfDecoration type = Decoration.TypeOfDecoration.build_01;

    public DecorationSerialized(int id, int floor_id, Decoration.TypeOfDecoration type)
    {
        this.id = id;
        this.floor_id = floor_id;
        this.type = type;
    }

    public DecorationSerialized(Decoration decoration_to_serialize)
    {
        floor_id = decoration_to_serialize.current_floor.id;
        type = decoration_to_serialize.model;
    }
}
