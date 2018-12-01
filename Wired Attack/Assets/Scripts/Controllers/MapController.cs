using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour
{
    #region variables

    public MachineController machineController = null;
    public MenuController editorModeMenuController = null;
    public GameController gameController = null;
    public DataController dataController = null;
    public PopUpTextController popTextController = null;

    public List<GameObject> floorPreFabs = new List<GameObject>();

    public enum TypeOfFloor
    {
        GRASS,
        SIDE_WALK
    }

    public enum SideOfFloor
    {
        TOP_LEFT,
        TOP_CENTER,
        TOP_RIGHT,
        LEFT,
        CENTER,
        RIGHT,
        BOTTOM_LEFT,
        BOTTOM_CENTER,
        BOTTOM_RIGHT
    }

    public GameController.GameMode currentGameMode = GameController.GameMode.DEMOSTRATION;

    public List<GameObject> connectionPreFabs = new List<GameObject>();
    public List<GameObject> machinePreFabs = new List<GameObject>();
    public List<GameObject> decorationPreFabs = new List<GameObject>();

    private IDictionary<TypeOfFloor,
                        IDictionary<SideOfFloor, GameObject>> floorGroups = new Dictionary<TypeOfFloor,
                                                                                            IDictionary<SideOfFloor, GameObject>>();
    private IDictionary<SideOfFloor, GameObject> grassFloors = new Dictionary<SideOfFloor, GameObject>();
    private IDictionary<SideOfFloor, GameObject> sideWalkFloors = new Dictionary<SideOfFloor, GameObject>();

    public GameObject floorEditorMenu = null;
    public GameObject machineEditorMenu = null;
    public GameObject decorationEditorMenu = null;

    public InputField mapTitleText = null;
    public InputField mapDescriptionText = null;

    public GameObject machinesHolder = null;
    public GameObject connectionsHolder = null;
    public GameObject decorationsHolder = null;
    public GameObject bitTextsHolder = null;

    private List<GameObject> tiles = new List<GameObject>();

    public GameObject selectedFloor = null;

    private Vector3 tileSize;

    public Map currentMap = null;

    public float gapSize = 0;
    public int REGULAR_MAP_WIDTH = 4;
    public int REGULAR_MAP_HEIGHT = 4;

    public int MENU_MAP_WIDTH = 20;
    public int MENU_MAP_HEIGHT = 30;

    private string typeOfConnectionToCreate = null; 

    #endregion

    void Start()
    {
        PrepareTilesInDictionaries();
    }

    void Update()
    {
        if (gameController.IsPaused()) return;
        if (currentGameMode == GameController.GameMode.EDIT_MODE)
        {
            DetectClickOnTiles();
        }
    }

    private void PrepareTilesInDictionaries()
    {
        List<GameObject> grassFloors = floorPreFabs.FindAll(floor_go => floor_go.GetComponent<Floor>().type == TypeOfFloor.GRASS);
        List<GameObject> sideWalkFloors = floorPreFabs.FindAll(floor_go => floor_go.GetComponent<Floor>().type == TypeOfFloor.SIDE_WALK);

        PrepareTilesInDictionary(this.grassFloors, grassFloors);
        PrepareTilesInDictionary(this.sideWalkFloors, sideWalkFloors);

        floorGroups.Add(TypeOfFloor.GRASS, this.grassFloors);
        floorGroups.Add(TypeOfFloor.SIDE_WALK, this.sideWalkFloors);
    }

    private void PrepareTilesInDictionary(IDictionary<SideOfFloor, GameObject> dictionary, List<GameObject> floors)
    {
        foreach (GameObject floorGo in floors)
        {
            Floor floor = floorGo.GetComponent<Floor>();
            dictionary.Add(floor.side, floorGo);
        }
    }

    public void ClearCurrentMap()
    {
        gameController.RemoveAllPlayers();

        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
        tiles.Clear();
    }

    //public void CreateFakeMap()
    //{
    //    CreateTiles(MENU_MAP_WIDTH, MENU_MAP_HEIGHT);
    //    DefineGameMode(GameController.GameMode.DEMOSTRATION);
    //    DefineMap(CreateAFakeMapObjects());
    //}

    public void DefineGameMode(GameController.GameMode gameMode)
    {
        currentGameMode = gameMode;
    }

    public void DefineMap(Map map)
    {
        currentMap = map;
    }

    public void CreateCurrentMap()
    {
        CreateTiles(REGULAR_MAP_WIDTH, REGULAR_MAP_HEIGHT);
        PlaceMachinesOfCurrentMap();
        DesactiveAllMenus();
    }

    private void CreateTiles(int mapWidth, int mapHeight)
    {
        tileSize = FloorAccordingPositionAndType(0, 0, 0, 0, TypeOfFloor.SIDE_WALK).GetComponent<Renderer>().bounds.size;

        float totalWidth = tileSize.x * mapWidth;
        float totalHeight = tileSize.y * mapHeight;

        Vector3 startPos = Vector3.zero;

        startPos.x = -((totalWidth / 2) - tileSize.x / 2);
        startPos.y = +((totalHeight / 2) - tileSize.x / 2);

        Vector3 currentPos = startPos;

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                GameObject floorGo = Instantiate(FloorAccordingPositionAndType(j, i, mapWidth, mapHeight, TypeOfFloor.SIDE_WALK));
                Floor floor = floorGo.GetComponent<Floor>();

                switch(currentGameMode)
                {
                    case GameController.GameMode.EDIT_MODE:
                        {
                            floor.TurnToEdit(true);
                            break;
                        }
                    case GameController.GameMode.PLAY_MODE:
                        {
                            floor.TurnToEdit(false);
                            break;
                        }
                    case GameController.GameMode.DEMOSTRATION:
                        {
                            floor.TurnToEdit(false);
                            break;
                        }
                }

                floorGo.transform.SetParent(transform);
                floorGo.transform.localPosition = currentPos;
                currentPos.x += tileSize.x;

                floorGo.transform.name = FormattedFloorName(j, i);
                floor.controller = gameObject;
                floor.id = tiles.Count;

                tiles.Add(floorGo);
            }
            currentPos.x = startPos.x;
            currentPos.y -= tileSize.y;
        }
    }

    private GameObject FloorAccordingPositionAndType(int posX, int posY, int maxWidth, int maxHeight, TypeOfFloor floorType)
    {
        GameObject floor = null;

        if (posY == 0)
        {
            if (posX == 0) { floor = floorGroups[floorType][SideOfFloor.TOP_LEFT]; }
            else if (posX == maxWidth - 1) { floor = floorGroups[floorType][SideOfFloor.TOP_RIGHT]; }
            else { floor = floorGroups[floorType][SideOfFloor.TOP_CENTER]; }
        }
        else if (posY == maxHeight - 1)
        {
            if (posX == 0) { floor = floorGroups[floorType][SideOfFloor.BOTTOM_LEFT]; }
            else if (posX == maxWidth - 1) { floor = floorGroups[floorType][SideOfFloor.BOTTOM_RIGHT]; }
            else { floor = floorGroups[floorType][SideOfFloor.BOTTOM_CENTER]; }
        }
        else
        {
            if (posX == 0) { floor = floorGroups[floorType][SideOfFloor.LEFT]; }
            else if (posX == maxWidth - 1) { floor = floorGroups[floorType][SideOfFloor.RIGHT]; }
            else { floor = floorGroups[floorType][SideOfFloor.CENTER]; }
        }

        return floor;
    }

    private string FormattedFloorName(int posX, int posY)
    {
        return string.Format("Tile {0} X {1}", posX + 1, posY + 1);
    }

    public void ConnectionModeOn(Connection connectionType)
    {
        typeOfConnectionToCreate = connectionType.wireType;
    }

    public void ConnectionModeOff()
    {
        typeOfConnectionToCreate = null;
    }

    private void DetectClickOnTiles()
    {
        if (!TouchHelpers.IsTouchingOrClickingOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit)
            {
                DesactiveAllMenus();

                if (hit.transform.tag == "floor")
                {
                    if (hit.transform.gameObject == selectedFloor)
                    {
                        DeselectCurrentFloor();
                        return;
                    }

                    if (selectedFloor != null)
                    {
                        if (typeOfConnectionToCreate != null)
                        {
                            Floor current_floor = CurrentSelectedFloor();
                            Floor another_floor = hit.transform.GetComponent<Floor>();

                            if (current_floor.IsHoldingSomething() &&
                                another_floor.IsHoldingSomething() &&
                                !machineController.IsThereConnectionBetween(current_floor.objectHolded,
                                                                             another_floor.objectHolded))
                            {
                                CreateConnectionBetweenMachinesOn(selectedFloor,
                                                                  hit.transform.gameObject,
                                                                  ConnectionPreFabByType(typeOfConnectionToCreate),
                                                                  ConnectionsOnMachines().Count);
                                ConnectionModeOff();
                                DeselectCurrentFloor();
                                return;
                            }
                        }

                        DeselectCurrentFloor();
                    }

                    SelectFloor(hit.transform.gameObject);

                    if (IsCurrentSelectedFloorHoldingSomething())
                    {
                        switch (selectedFloor.GetComponent<Floor>().objectHolded.tag)
                        {
                            case "machine":
                                {
                                    ActiveMachineEditorMenu(true);
                                    break;
                                }
                            case "decoration":
                                {
                                    ActiveDecorationEditorMenu(true);
                                    break;
                                }
                        }
                    }
                    else {
                        ActiveFloorEditorMenu(true);
                    }

                }
                else
                {
                    DeselectCurrentFloor();
                }
            }
        }
    }

    private void SelectFloor(GameObject floor)
    {
        selectedFloor = floor;
        CurrentSelectedFloor().ActiveBackGround(true);
    }

    private void DeselectCurrentFloor()
    {
        CurrentSelectedFloor().ActiveBackGround(false);
        selectedFloor = null;
    }

    public void NewMachine(GameObject machinePreFab)
    {
        GameObject machine = Instantiate(machinePreFab);
        PlaceNewMachineOnSelectedTile(machine);
    }

    private void PlaceNewMachineOnSelectedTile(GameObject machineGo)
    {
        Machine machine = machineGo.GetComponent<Machine>();
        machine.textParent = bitTextsHolder;
        machine.gameController = gameController;

        if (machine.id == 0)
        {
            machine.id = AvailableMachineId();
        }

        machineGo.transform.name = machine.title + machine.id.ToString();
        machineGo.transform.SetParent(machinesHolder.transform);

        GiveToSelectedFloor(machineGo);
        DeselectCurrentFloor();
    }

    private int AvailableMachineId()
    {
        int availableId = 1;
        while (MachinesOnTiles().Find(machine => machine.GetComponent<Machine>().id == availableId))
        {
            if (availableId >= REGULAR_MAP_HEIGHT * REGULAR_MAP_WIDTH)
                break;

            availableId++;
        }

        return availableId;
    }

    public void PlaceNewDecorationOnSelectedTile(GameObject decorationPreFab)
    {
        GameObject decorationGo = Instantiate(decorationPreFab);
        Decoration decoration = decorationGo.GetComponent<Decoration>();

        decoration.id = 0;
        decorationGo.transform.SetParent(decorationsHolder.transform);
        GiveToSelectedFloor(decorationGo);
        DeselectCurrentFloor();
    }

    private void GiveToSelectedFloor(GameObject objectToHold)
    {
        CurrentSelectedFloor().ReceiveObjectToHold(objectToHold);
    }

    public void RemoveFromSelectedFloor()
    {
        CurrentSelectedFloor().RemoveHoldedObject();
        DeselectCurrentFloor();
    }

    public void SetMachineTeam(int newTeam)
    {
        if (CurrentSelectedFloor().IsHoldingSomething())
        {
            Machine holdedMachine = CurrentSelectedFloor().objectHolded.GetComponent<Machine>();
            holdedMachine.ChangeOwner((TeamHelpers.Team)newTeam);
        }
        DeselectCurrentFloor();
    }

    public void CreateConnectionBetweenMachinesOn(GameObject firstFloor,
                                                  GameObject lastFloor,
                                                  GameObject wirePreFab,
                                                  int connection_id)
    {
        GameObject firstMachine = firstFloor.GetComponent<Floor>().objectHolded;
        GameObject lastMachine = lastFloor.GetComponent<Floor>().objectHolded;

        if (firstMachine.GetComponent<Machine>().CanHaveMoreConnections() &&
            lastMachine.GetComponent<Machine>().CanHaveMoreConnections())
        {
            GameObject connectionGo = Instantiate(wirePreFab);
            Connection connection = connectionGo.GetComponent<Connection>();

            connection.connectionsPoints[0] = firstMachine;
            connection.connectionsPoints[1] = lastMachine;
            connection.UpdatePointsOnLine();

            firstMachine.GetComponent<Machine>().connections.Add(connectionGo);
            lastMachine.GetComponent<Machine>().connections.Add(connectionGo);

            connection.id = connection_id;

            connectionGo.name = connection.wireType + connection.id.ToString();

            connectionGo.transform.SetParent(connectionsHolder.transform);
        }
        else
        {
            popTextController.CreatePopText("connections limit", firstMachine.transform);
        }
    }

    private bool IsCurrentSelectedFloorHoldingSomething()
    {
        return CurrentSelectedFloor().IsHoldingSomething();
    }

    public void SaveCurrentMap()
    {
        currentMap.ReplaceListOfMachines(MachinesOnTiles());
        currentMap.ReplaceListOfConnections(ConnectionsOnMachines());
        currentMap.ReplaceListOfDecorations(DecorationsOnTiles());

        currentMap.name = mapTitleText.text;
        currentMap.description = mapDescriptionText.text;

        dataController.SaveMap(currentMap);
    }

    //private Map CreateAFakeMapObjects()
    //{
    //    Map fake_map = new Map();
    //    int amount_of_decorations = MENU_MAP_HEIGHT * MENU_MAP_WIDTH;

    //    for (int i = 0; i < amount_of_decorations; i++)
    //    {
    //        fake_map.serialized_decorations.Add(new DecorationSerialized(i, i, Decoration.TypeOfDecoration.build_02));
    //    }

    //    return fake_map;
    //}
    
    public void PlaceMachinesOfCurrentMap()
    {
        foreach (MachineSerialized machineSerialized in currentMap.machinesSerializeds)
        {
            GameObject machineGo = Instantiate(MachinePreFabByModel(machineSerialized.machineModel));

            Machine machine = machineGo.GetComponent<Machine>();

            machine.id = machineSerialized.id;
            machine.team = (TeamHelpers.Team)machineSerialized.teamId;
            machine.model = machineSerialized.machineModel;
            selectedFloor = tiles[machineSerialized.floorId];

            PlaceNewMachineOnSelectedTile(machineGo);
        }

        foreach (ConnectionSerialized connectionSerialized in currentMap.connectionsSerializeds)
        {
            GameObject firstMachinePoint = MachinesOnTiles()
                .Find(machine =>machine.GetComponent<Machine>().id == connectionSerialized.connectionIds[0]);
            GameObject lastMachinePoint = MachinesOnTiles()
                .Find(machine => machine.GetComponent<Machine>().id == connectionSerialized.connectionIds[1]);

            GameObject firstFloor = firstMachinePoint.GetComponent<Machine>().currentFloor.transform.gameObject;
            GameObject lastFloor = lastMachinePoint.GetComponent<Machine>().currentFloor.transform.gameObject;

            CreateConnectionBetweenMachinesOn(firstFloor, lastFloor, ConnectionPreFabByType(connectionSerialized.wireType), connectionSerialized.id);
        }

        foreach (DecorationSerialized connectionSerialized in currentMap.decorationsSerializeds)
        {
            selectedFloor = tiles[connectionSerialized.floorId];
            PlaceNewDecorationOnSelectedTile(DecoractionPreFabByModel(connectionSerialized.type));

        }
        
        if (currentGameMode == GameController.GameMode.PLAY_MODE)
        {
            machineController.StartGame(MachinesOnTiles(), ConnectionsOnMachines());

            if (machineController.machines.Find(machine => machine.team == TeamHelpers.Team.HUMAN_TEAM))
                gameController.CreateHumanPlay();
            if (machineController.machines.Find(machine => machine.team == TeamHelpers.Team.RED_TEAM))
                gameController.CreateIAPlayer(TeamHelpers.Team.RED_TEAM);
            if (machineController.machines.Find(machine => machine.team == TeamHelpers.Team.YELLOW_TEAM))
                gameController.CreateIAPlayer(TeamHelpers.Team.YELLOW_TEAM);

            if (currentMap.tipTexts.Count > 0)
                gameController.Pause();
                popTextController.CreateTips(currentMap.tipTexts);
        } else if (currentGameMode == GameController.GameMode.EDIT_MODE)
        {
            mapTitleText.text = currentMap.name;
            mapDescriptionText.text = currentMap.description;
        }
    }

    private void DesactiveAllMenus()
    {
        ActiveFloorEditorMenu(false);
        ActiveMachineEditorMenu(false);
        ActiveDecorationEditorMenu(false);
    }

    private void ActiveFloorEditorMenu(bool activate)
    {
        if (activate)
        {
            editorModeMenuController.OpenMenuByObject(floorEditorMenu);
        }
        else
        {
            editorModeMenuController.CloseMenuByObject(floorEditorMenu);
        }
    }

    private void ActiveMachineEditorMenu(bool activate)
    {
        if (activate)
        {
            editorModeMenuController.OpenMenuByObject(machineEditorMenu);
        }
        else
        {
            editorModeMenuController.CloseMenuByObject(machineEditorMenu);
        }
    }

    private void ActiveDecorationEditorMenu(bool activate)
    {
        if (activate)
        {
            editorModeMenuController.OpenMenuByObject(decorationEditorMenu);
        }
        else
        {
            editorModeMenuController.CloseMenuByObject(decorationEditorMenu);
        }
    }

    private GameObject ConnectionPreFabByType(string wireName)
    {
        return connectionPreFabs.Find(connection => connection.GetComponent<Connection>().wireType == wireName);
    }

    private GameObject MachinePreFabByModel(string machineModel)
    {
        return machinePreFabs.Find(machine => machine.GetComponent<Machine>().model == machineModel);
    }

    private GameObject DecoractionPreFabByModel(Decoration.TypeOfDecoration model)
    {
        return decorationPreFabs.Find(decoration => decoration.GetComponent<Decoration>().model == model);
    }

    private Floor CurrentSelectedFloor()
    {
        return selectedFloor.GetComponent<Floor>();
    }

    private GameObject FloorById(int tileId)
    {
        return tiles.Find(tile => tile.GetComponent<Floor>().id == tileId);
    }

    private List<GameObject> OnTilesByTag(string tag)
    {
        List<GameObject> objectOnTiles = new List<GameObject>();

        foreach (GameObject floor in tiles)
        {
            GameObject holdedObject = floor.GetComponent<Floor>().objectHolded;
            if (holdedObject != null && holdedObject.tag == tag)
            {
                objectOnTiles.Add(holdedObject);
            }
        }

        return objectOnTiles;
    }

    private List<GameObject> MachinesOnTiles()
    {
        return OnTilesByTag("machine");
    }

    private List<GameObject> ConnectionsOnMachines()
    {
        List<GameObject> connectionsGos = new List<GameObject>();

        foreach (GameObject machineGo in MachinesOnTiles())
        {
            Machine machine = machineGo.GetComponent<Machine>();
            foreach (GameObject con in machine.connections)
            {
                if (!connectionsGos.Contains(con))
                {
                    connectionsGos.Add(con);
                }
            }
        }

        return connectionsGos;
    }

    private List<GameObject> DecorationsOnTiles()
    {
        return OnTilesByTag("decoration");
    }
}
