using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelController : MonoBehaviour {

    public GameController gameController = null;
    public MapController mapController = null;
    public DataController dataController = null;

    public enum TypeOfMaps
    {
        CAMPAIGN_MAPS,
        CUSTOM_MAPS
    }

    public TypeOfMaps list_of = TypeOfMaps.CAMPAIGN_MAPS;
    public GameController.GameMode startGameIn = GameController.GameMode.PLAY_MODE;

    public GameObject list = null;
    public GameObject chooseLevelBtnPreFab = null;
    public GameObject newLevelBtnPreFab = null;
    public GameObject playerBtn = null;
    
    public Map selectedMap = null;
    
	void Start () { }
	
	void Update () { }

    public void StartGame()
    {
        mapController.ClearCurrentMap();
        mapController.DefineGameMode(startGameIn);
        mapController.DefineMap(selectedMap);
        mapController.CreateCurrentMap();
    }

    public void SelectLevel(Map map)
    {
        if (selectedMap == map)
        {
            selectedMap = null;
            AllowGoToGame(false);
        } else {
            selectedMap = map;
            AllowGoToGame(true);
        }
    }

    private void AllowGoToGame(bool allow)
    {
        playerBtn.SetActive(allow);
    }

    private void CreateAllLevelButtons()
    {
        IDictionary<string, List<string>> rawMaps = new Dictionary<string, List<string>>();

        switch (list_of)
        {
            case TypeOfMaps.CAMPAIGN_MAPS:
                {
                    rawMaps = dataController.AllRawDataOfAllCampaignMaps();
                    break;
                }
            case TypeOfMaps.CUSTOM_MAPS:
                {
                    rawMaps = dataController.AllRawDataOfAllCustomMaps();
                    break;
                }
        }

        bool previusLevelFinished = true;
        foreach (KeyValuePair<string, List<string>> rawMap in rawMaps)
        {
            Map map = new Map(rawMap.Value);
            map.fileName = rawMap.Key;

            GameObject newBtn = CreateLevelButton(map);

            if (list_of != TypeOfMaps.CAMPAIGN_MAPS)
                continue;

            newBtn.GetComponent<Button>().interactable = previusLevelFinished;
            previusLevelFinished = newBtn.GetComponent<SelectLevel>().mapScore.totalTime > 0;
        }

        if (startGameIn == GameController.GameMode.EDIT_MODE)
        {
            Map map = new Map(new List<string>());
            map.fileName = map.name = DataController.NEW_GAME_KEY;

            CreateLevelButton(map);
        }
    }

    private void DeleteAllLevelButtons()
    {
        Transform listTransform = list.transform;
        foreach(Transform child in listTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private GameObject CreateLevelButton(Map mapTarget)
    {
        Transform listTransform = list.transform;

        GameObject btnPreFab = chooseLevelBtnPreFab;
        if (mapTarget.name == DataController.NEW_GAME_KEY)
            btnPreFab = newLevelBtnPreFab;

        GameObject newBtn = Instantiate(btnPreFab);

        newBtn.name = mapTarget.name;
        newBtn.transform.SetParent(listTransform);
        newBtn.transform.localScale = new Vector3(1, 1, 1);

        SelectLevel select_level = newBtn.GetComponent<SelectLevel>();
        select_level.mapTarget = mapTarget;
        select_level.selectLevelController = this;
        select_level.DefineButtonStatus();

        return newBtn;
    }

    private void OnEnable()
    {
        DeleteAllLevelButtons();
        CreateAllLevelButtons();
        SelectLevel(selectedMap);
    }
}
