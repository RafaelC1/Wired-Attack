using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour {

    public GameObject level_blocked_icon = null;
    public GameObject level_desblocked_icon = null;
    public GameObject level_finished_icon = null;

    public Text title = null;
    public Text description = null;

    public SelectLevelController select_level_controller = null;
    public Map map_target = null;
	
	void Start ()
    {
    }	
	
	void Update () { }

    public void DefineButtonStatus()
    {
        DefineTitle(map_target.name);
        DefineDescription(map_target.description);

        switch(map_target.map_status)
        {
            case 0:
                {
                    BlockLevel();
                    break;
                }
            case 1:
                {
                    UnblockLevel();
                    break;
                }
            case 2:
                {
                    PassLevel();
                    break;
                }
            default:
                {
                    DisableAllStatusIcons();
                    break;
                }
        }
    }


    private void DefineTitle(string new_title)
    {
        title.text = new_title;
    }

    private void DefineDescription(string new_description)
    {
        if (description != null)
        {
            description.text = new_description;
        }
    }

    private void DisableAllStatusIcons()
    {
        if (level_blocked_icon != null) { level_blocked_icon.SetActive(false); }
        if (level_desblocked_icon != null) { level_desblocked_icon.SetActive(false); }
        if (level_finished_icon != null) { level_finished_icon.SetActive(false); }
    }

    public void SelectThisLevel()
    {
        select_level_controller.SelectLevel(map_target);
    }

    public void BlockLevel()
    {
        level_blocked_icon.SetActive(true);
        GetComponent<Button>().enabled = false;
    }

    public void UnblockLevel()
    {
        level_desblocked_icon.SetActive(true);
    }

    public void PassLevel()
    {
        level_finished_icon.SetActive(true);
    }
}
