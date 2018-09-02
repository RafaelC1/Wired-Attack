using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelController : MonoBehaviour {


    public GameController game_controller = null;
    public MapController map_controller = null;
    public DataController data_controller = null;

    public GameController.GameStatus game_mode = GameController.GameStatus.GAME_MODE;
    public GameController.GameStatus list_of_level_mode = GameController.GameStatus.GAME_MODE;

    public GameObject list = null;
    public GameObject level_btn_pre_fab = null;
    public GameObject play_btn = null;

    private List<string> level_names = new List<string>();
    private string selected_level = null;
    
	void Start ()
    {
        LoadAllLevelNames();
        CreateAllLevelButtons();
    }
	
	void Update () { }

    public void StartGame()
    {
        map_controller.ClearMap();
        map_controller.CreateMap(game_mode);
        map_controller.LoadMapByName(selected_level, list_of_level_mode);
    }

    public void SelectLevel(string level_name)
    {
        if (selected_level == level_name)
        {
            selected_level = null;
            AllowGoToGame(false);
        } else {
            selected_level = level_name;
            AllowGoToGame(true);
        }
    }

    private void AllowGoToGame(bool allow)
    {
        play_btn.SetActive(allow);
    }

    private void LoadAllLevelNames()
    {
        List<string> level_names = data_controller.AllLevelNames(list_of_level_mode);

        foreach(string level_name in level_names)
        {
            string[] all_parts = level_name.Split('\\');
            string full_name = all_parts[all_parts.Length-1];
            this.level_names.Add(full_name.Split('.')[0]);
        }
    }

    private void CreateAllLevelButtons()
    {
        foreach (string level_name in level_names)
        {
            CreateNewlevelButton(level_name);
        }

        if (game_mode == GameController.GameStatus.EDIT_MODE)
        {
            CreateNewlevelButton(DataController.NEW_GAME_KEY);
        }
    }

    private void CreateNewlevelButton(string level_name_to_target)
    {
        Transform list_transform = list.transform;

        GameObject new_button = Instantiate(level_btn_pre_fab);

        new_button.name = level_name_to_target;
        new_button.transform.name = level_name_to_target;
        new_button.transform.SetParent(list_transform);
        new_button.transform.localScale = new Vector3(1, 1, 1);
        new_button.transform.GetChild(0).GetComponent<Text>().text = level_name_to_target;

        SelectLevel select_level = new_button.GetComponent<SelectLevel>();
        select_level.level_target = level_name_to_target;
        select_level.select_level_controller = this;
    }

    private void OnEnable()
    {
        SelectLevel(selected_level);
    }
}
