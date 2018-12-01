using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSerialized : Serializable {

    public Decoration.TypeOfDecoration type = Decoration.TypeOfDecoration.BUILD_01;

    public DecorationSerialized(int id, int floorId, Decoration.TypeOfDecoration type)
    {
        this.id = id;
        this.floorId = floorId;
        this.type = type;
    }

    public DecorationSerialized(Decoration decorationToSerialized)
    {
        floorId = decorationToSerialized.currentFloor.id;
        type = decorationToSerialized.model;
    }
}
