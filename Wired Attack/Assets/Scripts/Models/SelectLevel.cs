using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour {
    
    public Text title = null;
    public Text description = null;

    public SelectLevelController select_level_controller = null;
    public Map map_target = null;

    public FinalScore map_score = null;
	
	void Start ()
    {
    }	
	
	void Update () { }

    public void DefineButtonStatus()
    {
        DefineTitle(map_target.name);
        DefineDescription(map_target.description);
        DefineFinalScore();
    }


    private void DefineTitle(string new_title)
    {
        if (new_title != DataController.NEW_GAME_KEY)
            title.text = new_title;
    }

    private void DefineDescription(string new_description)
    {
        if (description != null)
            description.text = new_description;
    }

    private void DefineFinalScore()
    {
        if (map_score == null)
            return;
        map_score.DefineMapName(map_target.name);
        map_score.LoadNestTime();
    }
    
    public void SelectThisLevel()
    {
        select_level_controller.SelectLevel(map_target);
    }
    
}
