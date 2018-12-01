using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour {
    
    public Text title = null;
    public Text description = null;

    public SelectLevelController selectLevelController = null;
    public Map mapTarget = null;

    public FinalScore mapScore = null;
	
	void Start ()
    {
    }	
	
	void Update () { }

    public void DefineButtonStatus()
    {
        DefineTitle(mapTarget.name);
        DefineDescription(mapTarget.description);
        DefineFinalScore();
    }


    private void DefineTitle(string newTitle)
    {
        if (newTitle != DataController.NEW_GAME_KEY)
            title.text = newTitle;
    }

    private void DefineDescription(string newDescription)
    {
        if (description != null)
            description.text = newDescription;
    }

    private void DefineFinalScore()
    {
        if (mapScore == null)
            return;
        mapScore.DefineMapName(mapTarget.name);
        mapScore.LoadNestTime();
    }
    
    public void SelectThisLevel()
    {
        selectLevelController.SelectLevel(mapTarget);
    }
    
}
