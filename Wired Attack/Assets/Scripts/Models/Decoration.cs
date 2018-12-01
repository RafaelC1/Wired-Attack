using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : Holdable {

    public enum TypeOfDecoration
    {
        BUILD_01,
        BUILD_02,
        BUILD_03
    }

    public TypeOfDecoration model = TypeOfDecoration.BUILD_01;
    
	void Start () { }
	
	void Update () { }
}
