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
    
    public Map selected_map = null;
    
	void Start ()
    {
        DeleteAllLevelButtons();
        CreateAllLevelButtons();
        SelectLevel(selected_map);
    }
	
	void Update () { }

    public void StartGame()
    {
        map_controller.ClearCurrentMap();
        map_controller.DefineGameMode(start_game_in);
        map_controller.DefineMap(selected_map);
        map_controller.CreateCurrentMap();
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

        foreach (KeyValuePair<string, List<string>> raw_map in raw_maps)
        {
            Map map = new Map(raw_map.Value);
            map.file_name = raw_map.Key;

            CreateLevelButton(map);
        }

        if (start_game_in == GameController.GameMode.EDIT_MODE)
        {
            Map map = new Map(new List<string>());
            map.file_name = map.name = DataController.NEW_GAME_KEY;           

            CreateLevelButton(map);
        }

        if (list_of == TypeOfMaps.CAMPAIGN_MAPS)
            DisableForCampaign();
    }

    private void DeleteAllLevelButtons()
    {
        Transform list_transform = list.transform;
        foreach(Transform child in list_transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateLevelButton(Map map_target)
    {
        Transform list_transform = list.transform;

        GameObject btn_pre_fab = choose_level_btn_pre_fab;
        if (map_target.name == DataController.NEW_GAME_KEY)
            btn_pre_fab = new_level_btn_pre_fab;

        GameObject new_button = Instantiate(btn_pre_fab);

        new_button.name = map_target.name;
        new_button.transform.SetParent(list_transform);
        new_button.transform.localScale = new Vector3(1, 1, 1);

        SelectLevel select_level = new_button.GetComponent<SelectLevel>();
        select_level.map_target = map_target;
        select_level.select_level_controller = this;
        select_level.DefineButtonStatus();
    }

    private void DisableForCampaign()
    {
        bool previus_level_finished = true;
        foreach (Transform btn in list.transform)
        {
            Button button = btn.GetComponent<Button>();
            button.interactable = previus_level_finished;
            previus_level_finished = btn.GetComponent<SelectLevel>().map_score.total_time > 0;
        }
    }

    private void OnEnable()
    {
        DisableForCampaign();
        if (list_of == TypeOfMaps.CAMPAIGN_MAPS)
            return;
        DeleteAllLevelButtons();
        CreateAllLevelButtons();
        SelectLevel(selected_map);
    }
}
