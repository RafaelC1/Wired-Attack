using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSerialized : Serializable {

    public MapController.TypeOfDecoration type = MapController.TypeOfDecoration.build_01;

    public DecorationSerialized(Decoration decoration_to_serialize)
    {
        floor_id = decoration_to_serialize.current_floor.id;
        type = decoration_to_serialize.model;
    }
}
