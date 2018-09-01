using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {

    public GameController.GameStatus list_from = GameController.GameStatus.GAME_MODE;

    public GameObject list = null;
    public GameObject level_btn_pre_fab = null;

    public MapController map_controller = null;

    private List<string> level_names = new List<string>();

    public DataController data_controller = null;
    
	void Start ()
    {
        LoadAllLevelNames();
        CreateAllLevelButtons();
    }
	
	void Update () { }

    private void LoadAllLevelNames()
    {
        List<string> level_names = data_controller.AllLevelNames(list_from);

        foreach(string level_name in level_names)
        {
            if (level_name.Contains(".meta")) { continue; }

            string[] all_parts = level_name.Split('\\');
            string full_name = all_parts[all_parts.Length-1];
            this.level_names.Add(full_name.Split('.')[0]);
        }
        this.level_names.Add("NEW_MAP");
    }

    private void CreateAllLevelButtons()
    {
        Transform list_transform = list.transform;

        foreach (string level_name in level_names)
        {
            GameObject new_button = Instantiate(level_btn_pre_fab);

            new_button.name = level_name;
            new_button.transform.name = level_name;
            new_button.transform.SetParent(list_transform);
            new_button.transform.localScale = new Vector3(1, 1, 1);
            new_button.transform.GetChild(0).GetComponent<Text>().text = level_name;

            Button button_script = new_button.GetComponent<UnityEngine.UI.Button>();
            button_script.onClick.AddListener(() => map_controller.DefineNextMap(level_name));
        }
    }
}
