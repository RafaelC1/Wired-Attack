using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour {

    public SelectLevelController select_level_controller = null;
    public string level_target = DataController.NEW_GAME_KEY;
	
	void Start () {	}	
	
	void Update () { }

    public void SelectThisLevel()
    {
        select_level_controller.SelectLevel(level_target);
    }
}
