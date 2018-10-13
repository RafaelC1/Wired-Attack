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
    
    private Map selected_map = null;
    
	void Start ()
    {
        CreateAllLevelButtons();
    }
	
	void Update () { }

    public void StartGame()
    {
        map_controller.ClearMap();
        map_controller.CreateMap(game_mode);
        map_controller.LoadMap(selected_map, game_mode);
    }

    public void SelectLevel(Map map)
    {
        if (selected_map == map)
        {
            selected_map = null;
            AllowGoToGame(false);
        } else {
            selected_map = map;
            AllowGoToGame(true);
        }
    }

    private void AllowGoToGame(bool allow)
    {
        play_btn.SetActive(allow);
    }

    private void CreateAllLevelButtons()
    {
        IDictionary<string, List<string>> raw_maps = new Dictionary<string, List<string>>();

        switch (list_of_level_mode)
        {
            case GameController.GameStatus.GAME_MODE:
                {
                    raw_maps = data_controller.AllRawDataOfAllCampaignMaps();
                    break;
                }
            case GameController.GameStatus.EDIT_MODE:
                {
                    raw_maps = data_controller.AllRawDataOfAllCustomMaps();
                    break;
                }
        }

        if (game_mode == GameController.GameStatus.EDIT_MODE)
        {
            raw_maps.Add(DataController.NEW_GAME_KEY, new List<string>());
        }

        foreach (KeyValuePair<string, List<string>> raw_map in raw_maps)
        {
            Map map = new Map(raw_map.Value);
            map.name = raw_map.Key;

            CreateNewlevelButton(map);
        }

    }

    private void CreateNewlevelButton(Map map_target)
    {
        Transform list_transform = list.transform;

        GameObject new_button = Instantiate(level_btn_pre_fab);

        new_button.name = map_target.name;
        new_button.transform.name = map_target.name;
        new_button.transform.SetParent(list_transform);
        new_button.transform.localScale = new Vector3(1, 1, 1);
        new_button.transform.GetChild(0).GetComponent<Text>().text = map_target.name;

        SelectLevel select_level = new_button.GetComponent<SelectLevel>();
        select_level.map_target = map_target;
        select_level.select_level_controller = this;
    }

    private void OnEnable()
    {
        SelectLevel(selected_map);
    }
}
