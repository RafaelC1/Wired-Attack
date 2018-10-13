using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour {

    public SelectLevelController select_level_controller = null;
    public Map map_target = null;
	
	void Start () {	}	
	
	void Update () { }

    public void SelectThisLevel()
    {
        select_level_controller.SelectLevel(map_target);
    }
}
