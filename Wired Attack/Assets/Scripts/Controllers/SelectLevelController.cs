using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelController : MonoBehaviour {


    public GameController game_controller = null;
    public MapController map_controller = null;
    public DataController data_controller = null;

    public enum TypeOfMaps
    {
        CAMPAIGN_MAPS,
        CUSTOM_MAPS
    }

    public TypeOfMaps list_of = TypeOfMaps.CAMPAIGN_MAPS;
    public GameController.GameMode start_game_in = GameController.GameMode.PLAY_MODE;

    public GameObject list = null;
    public GameObject choose_level_btn_pre_fab = null;
    public GameObject new_level_btn_pre_fab = null;
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
        map_controller.CreateMap(start_game_in);
        map_controller.LoadMap(selected_map, start_game_in);
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

        switch (list_of)
        {
            case TypeOfMaps.CAMPAIGN_MAPS:
                {
                    raw_maps = data_controller.AllRawDataOfAllCampaignMaps();
                    break;
                }
            case TypeOfMaps.CUSTOM_MAPS:
                {
                    raw_maps = data_controller.AllRawDataOfAllCustomMaps();
                    break;
                }
        }

        if (start_game_in == GameController.GameMode.EDIT_MODE)
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

        GameObject btn_pre_fab = choose_level_btn_pre_fab;

        if (map_target.name == DataController.NEW_GAME_KEY)
        {
            btn_pre_fab = new_level_btn_pre_fab;
        }

        GameObject new_button = Instantiate(btn_pre_fab);

        new_button.name = map_target.name;
        new_button.transform.SetParent(list_transform);
        new_button.transform.localScale = new Vector3(1, 1, 1);
        new_button.transform.GetChild(0).GetComponent<Text>().text = map_target.name;

        SelectLevel select_level = new_button.GetComponent<SelectLevel>();
        select_level.map_target = map_target;
        select_level.select_level_controller = this;
        select_level.DefineButtonStatus();
    }

    private void OnEnable()
    {
        SelectLevel(selected_map);
    }
}
