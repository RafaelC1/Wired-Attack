using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : Holdable {

    public enum TypeOfDecoration
    {
        build_01,
        build_02,
        build_03
    }

    public TypeOfDecoration model = TypeOfDecoration.build_01;
    
	void Start () { }
	
	void Update () { }
}
