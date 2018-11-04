using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour
{

    #region variables

    public MachineController machine_controller = null;
    public MenuController editor_mode_menus = null;
    public GameController game_controller = null;
    public DataController data_controller = null;
    public PopUpTextController pop_text_controller = null;

    public List<GameObject> floor_pre_fabs = new List<GameObject>();

    public enum TypeOfFloor
    {
        grass,
        side_walk
    }

    public enum SideOfFloor
    {
        top_left,
        top_center,
        top_right,
        left,
        center,
        right,
        bottom_left,
        bottom_center,
        bottom_right
    }

    public List<GameObject> connection_pre_fabs = new List<GameObject>();
    public List<GameObject> machines_pre_fabs = new List<GameObject>();
    public List<GameObject> decoractions_pre_fabs = new List<GameObject>();

    private IDictionary<TypeOfFloor,
                        IDictionary<SideOfFloor, GameObject>> floor_groups = new Dictionary<TypeOfFloor,
                                                                                            IDictionary<SideOfFloor, GameObject>>();
    private IDictionary<SideOfFloor, GameObject> grass_floors = new Dictionary<SideOfFloor, GameObject>();
    private IDictionary<SideOfFloor, GameObject> side_walk_floors = new Dictionary<SideOfFloor, GameObject>();

    public GameObject floor_editor_menu = null;
    public GameObject machine_editor_menu = null;
    public GameObject decoration_editor_menu = null;

    public GameObject machine_parent = null;
    public GameObject connection_parent = null;
    public GameObject decoration_parent = null;
    public GameObject canvas = null;

    private List<GameObject> tiles = new List<GameObject>();

    public GameObject selected_floor = null;

    private Vector3 tile_size;

    public string current_map_name = "";

    public float gap_size = 0;
    public int REGULAR_MAP_WIDTH = 4;
    public int REGULAR_MAP_HEIGHT = 4;

    public int MENU_MAP_WIDTH = 20;
    public int MENU_MAP_HEIGHT = 30;

    private GameController.GameMode current_status;

    #endregion

    void Start()
    {
        PrepareTilesInDictionaries();
        CreateFakeMap();
    }

    void Update()
    {
        if (current_status == GameController.GameMode.EDIT_MODE)
        {
            DetectClickOnTiles();
        }
    }

    private void PrepareTilesInDictionaries()
    {
        List<GameObject> grass_floors = floor_pre_fabs.FindAll(floor_go => floor_go.GetComponent<Floor>().type == TypeOfFloor.grass);
        List<GameObject> side_walk_floors = floor_pre_fabs.FindAll(floor_go => floor_go.GetComponent<Floor>().type == TypeOfFloor.side_walk);

        PrepareTilesInDictionary(this.grass_floors, grass_floors);
        PrepareTilesInDictionary(this.side_walk_floors, side_walk_floors);

        floor_groups.Add(TypeOfFloor.grass, this.grass_floors);
        floor_groups.Add(TypeOfFloor.side_walk, this.side_walk_floors);
    }

    private void PrepareTilesInDictionary(IDictionary<SideOfFloor, GameObject> dictionary, List<GameObject> floors)
    {
        foreach (GameObject floor_go in floors)
        {
            Floor floor = floor_go.GetComponent<Floor>();
            dictionary.Add(floor.side, floor_go);
        }
    }

    public void ClearCurrentMap()
    {
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
        tiles.Clear();
    }

    public void CreateFakeMap()
    {
        CreateTiles(GameController.GameMode.DEMOSTRATION, MENU_MAP_WIDTH, MENU_MAP_HEIGHT);
        LoadMap(CreateAFakeMapObjects(), GameController.GameMode.DEMOSTRATION);
    }

    public void CreateGameMap(GameController.GameMode game_mode)
    {
        CreateTiles(game_mode, REGULAR_MAP_WIDTH, REGULAR_MAP_HEIGHT);
        DesactiveAllMenus();
    }

    private void CreateTiles(GameController.GameMode game_mode, int map_width, int map_height)
    {
        tile_size = FloorAccordingPositionAndType(0, 0, 0, 0, TypeOfFloor.side_walk).GetComponent<Renderer>().bounds.size;

        float total_width = tile_size.x * map_width;
        float total_height = tile_size.y * map_height;

        Vector3 start_pos = Vector3.zero;

        start_pos.x = -((total_width / 2) - tile_size.x / 2);
        start_pos.y = +((total_height / 2) - tile_size.x / 2);

        Vector3 current_pos = start_pos;

        for (int i = 0; i < map_height; i++)
        {
            for (int j = 0; j < map_width; j++)
            {
                GameObject floor_go = Instantiate(FloorAccordingPositionAndType(j, i, map_width, map_height, TypeOfFloor.side_walk));
                Floor floor = floor_go.GetComponent<Floor>();

                switch(game_mode)
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

                floor_go.transform.SetParent(transform);
                floor_go.transform.localPosition = current_pos;
                current_pos.x += tile_size.x;

                floor_go.transform.name = FormattedFloorName(j, i);
                floor.controller = gameObject;
                floor.id = tiles.Count;

                tiles.Add(floor_go);
            }
            current_pos.x = start_pos.x;
            current_pos.y -= tile_size.y;
        }
    }

    private GameObject FloorAccordingPositionAndType(int pos_x, int pos_y, int max_width, int max_height, TypeOfFloor floor_type)
    {
        GameObject floor = null;

        if (pos_y == 0)
        {
            if (pos_x == 0) { floor = floor_groups[floor_type][SideOfFloor.top_left]; }
            else if (pos_x == max_width - 1) { floor = floor_groups[floor_type][SideOfFloor.top_right]; }
            else { floor = floor_groups[floor_type][SideOfFloor.top_center]; }
        }
        else if (pos_y == max_height - 1)
        {
            if (pos_x == 0) { floor = floor_groups[floor_type][SideOfFloor.bottom_left]; }
            else if (pos_x == max_width - 1) { floor = floor_groups[floor_type][SideOfFloor.bottom_right]; }
            else { floor = floor_groups[floor_type][SideOfFloor.bottom_center]; }
        }
        else
        {
            if (pos_x == 0) { floor = floor_groups[floor_type][SideOfFloor.left]; }
            else if (pos_x == max_width - 1) { floor = floor_groups[floor_type][SideOfFloor.right]; }
            else { floor = floor_groups[floor_type][SideOfFloor.center]; }
        }

        return floor;
    }

    private string FormattedFloorName(int pos_x, int pos_y)
    {
        return string.Format("Tile {0} X {1}", pos_x + 1, pos_y + 1);
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
                    if (hit.transform.gameObject == selected_floor)
                    {
                        DeselectCurrentFloor();
                        return;
                    }

                    if (selected_floor != null)
                    {
                        Floor current_floor = CurrentSelectedFloor();
                        Floor another_floor = hit.transform.GetComponent<Floor>();

                        if (current_floor.IsHoldingSomething() &&
                            another_floor.IsHoldingSomething() &&
                            !machine_controller.IsThereConnectionBetween(current_floor.object_holded,
                                                                         another_floor.object_holded))
                        {
                            CreateConnectionBetweenMachinesOn(selected_floor,
                                                              hit.transform.gameObject,
                                                              ConnectionPreFabByType("optical"),
                                                              ConnectionsOnMachines().Count);
                            DeselectCurrentFloor();
                            return;
                        }

                        DeselectCurrentFloor();
                    }

                    SelectFloor(hit.transform.gameObject);

                    if (IsCurrentSelectedFloorHoldingSomething())
                    {
                        switch (selected_floor.GetComponent<Floor>().object_holded.tag)
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
        selected_floor = floor;
        CurrentSelectedFloor().ActiveBackGround(true);
    }

    private void DeselectCurrentFloor()
    {
        CurrentSelectedFloor().ActiveBackGround(false);
        selected_floor = null;
    }

    public void NewMachine(GameObject machine_pre_fab)
    {
        GameObject machine = Instantiate(machine_pre_fab);
        PlaceNewMachineOnSelectedTile(machine);
    }

    private void PlaceNewMachineOnSelectedTile(GameObject machine_go)
    {
        Machine machine = machine_go.GetComponent<Machine>();
        machine.canvas = canvas;

        if (machine.id == 0)
        {
            machine.id = MachinesOnTiles().Count + 1;
        }

        machine_go.transform.name = machine.model + machine.id.ToString();
        machine_go.transform.SetParent(machine_parent.transform);

        GiveToSelectedFloor(machine_go);
        DeselectCurrentFloor();
    }

    public void PlaceNewDecorationOnSelectedTile(GameObject decoration_pre_fab)
    {
        GameObject decoration_go = Instantiate(decoration_pre_fab);
        Decoration decoration = decoration_go.GetComponent<Decoration>();

        decoration.id = 0;
        decoration_go.transform.SetParent(decoration_parent.transform);
        GiveToSelectedFloor(decoration_go);
        DeselectCurrentFloor();
    }

    private void GiveToSelectedFloor(GameObject object_to_hold)
    {
        CurrentSelectedFloor().ReceiveObjectToHold(object_to_hold);
    }

    public void RemoveFromSelectedFloor()
    {
        CurrentSelectedFloor().RemoveHoldedObject();
        DeselectCurrentFloor();
    }

    public void SetMachineTeam(int new_team)
    {
        if (CurrentSelectedFloor().IsHoldingSomething())
        {
            Machine holded_machine = CurrentSelectedFloor().object_holded.GetComponent<Machine>();
            holded_machine.ChangeOwner((TeamHelpers.Team)new_team);
        }
        DeselectCurrentFloor();
    }

    public void CreateConnectionBetweenMachinesOn(GameObject first_floor,
                                                  GameObject secound_floor,
                                                  GameObject wire_pre_fab,
                                                  int connection_id)
    {
        GameObject first_machine = first_floor.GetComponent<Floor>().object_holded;
        GameObject secound_machine = secound_floor.GetComponent<Floor>().object_holded;

        if (first_machine.GetComponent<Machine>().CanHaveMoreConnections() &&
            secound_machine.GetComponent<Machine>().CanHaveMoreConnections())
        {
            GameObject connection_go = Instantiate(wire_pre_fab);
            Connection connection = connection_go.GetComponent<Connection>();

            connection.connection_points[0] = first_machine;
            connection.connection_points[1] = secound_machine;

            first_machine.GetComponent<Machine>().connections.Add(connection_go);
            secound_machine.GetComponent<Machine>().connections.Add(connection_go);

            connection.id = connection_id;

            connection_go.name = connection.wire_type + connection.id.ToString();

            connection_go.transform.SetParent(connection_parent.transform);
        }
        else
        {
            pop_text_controller.CreatePopText("connections limit", first_machine.transform);
        }
    }

    private bool IsCurrentSelectedFloorHoldingSomething()
    {
        return CurrentSelectedFloor().IsHoldingSomething();
    }

    public void SaveCurrentMap()
    {
        Map map_to_save = new Map(MachinesOnTiles(), ConnectionsOnMachines(), DecorationsOnTiles());
        map_to_save.name = current_map_name;

        data_controller.SaveMap(map_to_save);
    }

    private Map CreateAFakeMapObjects()
    {
        Map fake_map = new Map();
        int amount_of_decorations = MENU_MAP_HEIGHT * MENU_MAP_WIDTH;

        for (int i = 0; i < amount_of_decorations; i++)
        {
            fake_map.serialized_decorations.Add(new DecorationSerialized(i, i, Decoration.TypeOfDecoration.build_02));
        }

        return fake_map;
    }
    
    public void LoadMap(Map map_to_load, GameController.GameMode current_game_status)
    {
        foreach (MachineSerialized machine_s in map_to_load.serialized_machines)
        {
            GameObject machine_go = Instantiate(MachinePreFabByModel(machine_s.machine_model));

            Machine machine = machine_go.GetComponent<Machine>();

            machine.id = machine_s.id;
            machine.team = (TeamHelpers.Team)machine_s.team_id;
            machine.model = machine_s.machine_model;
            selected_floor = tiles[machine_s.floor_id];

            PlaceNewMachineOnSelectedTile(machine_go);
        }

        foreach (ConnectionSerialized con_s in map_to_load.serialized_connections)
        {
            GameObject first_machine_point = MachinesOnTiles().Find(machine => machine.GetComponent<Machine>().id == con_s.connection_ids[0]);
            GameObject secound_machine_point = MachinesOnTiles().Find(machine => machine.GetComponent<Machine>().id == con_s.connection_ids[1]);

            GameObject first_floor = first_machine_point.GetComponent<Machine>().current_floor.transform.gameObject;
            GameObject secound_floor = secound_machine_point.GetComponent<Machine>().current_floor.transform.gameObject;

            CreateConnectionBetweenMachinesOn(first_floor, secound_floor, ConnectionPreFabByType(con_s.wire_type), con_s.id);
        }

        foreach (DecorationSerialized deco_s in map_to_load.serialized_decorations)
        {
            selected_floor = tiles[deco_s.floor_id];
            PlaceNewDecorationOnSelectedTile(DecoractionPreFabByModel(deco_s.type));

        }

        current_status = current_game_status;
        current_map_name = map_to_load.name;

        if (current_game_status == GameController.GameMode.PLAY_MODE)
        {
            machine_controller.StartGame(MachinesOnTiles(), ConnectionsOnMachines());
            game_controller.CreatePlayers();
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
            editor_mode_menus.OpenMenuByObject(floor_editor_menu);
        }
        else
        {
            editor_mode_menus.CloseMenuByObject(floor_editor_menu);
        }
    }

    private void ActiveMachineEditorMenu(bool activate)
    {
        if (activate)
        {
            editor_mode_menus.OpenMenuByObject(machine_editor_menu);
        }
        else
        {
            editor_mode_menus.CloseMenuByObject(machine_editor_menu);
        }
    }

    private void ActiveDecorationEditorMenu(bool activate)
    {
        if (activate)
        {
            editor_mode_menus.OpenMenuByObject(decoration_editor_menu);
        }
        else
        {
            editor_mode_menus.CloseMenuByObject(decoration_editor_menu);
        }
    }

    private GameObject ConnectionPreFabByType(string wire_name)
    {
        return connection_pre_fabs.Find(connection => connection.GetComponent<Connection>().wire_type == wire_name);
    }

    private GameObject MachinePreFabByModel(string machine_model)
    {
        return machines_pre_fabs.Find(machine => machine.GetComponent<Machine>().model == machine_model);
    }

    private GameObject DecoractionPreFabByModel(Decoration.TypeOfDecoration model)
    {
        return decoractions_pre_fabs.Find(decoration => decoration.GetComponent<Decoration>().model == model);
    }

    private Floor CurrentSelectedFloor()
    {
        return selected_floor.GetComponent<Floor>();
    }

    private GameObject FloorById(int tile_id)
    {
        return tiles.Find(tile => tile.GetComponent<Floor>().id == tile_id);
    }

    private List<GameObject> OnTilesByTag(string tag)
    {
        List<GameObject> objects_on_tiles = new List<GameObject>();

        foreach (GameObject floor in tiles)
        {
            GameObject holded_object = floor.GetComponent<Floor>().object_holded;
            if (holded_object != null && holded_object.tag == tag)
            {
                objects_on_tiles.Add(holded_object);
            }
        }

        return objects_on_tiles;
    }

    private List<GameObject> MachinesOnTiles()
    {
        return OnTilesByTag("machine");
    }

    private List<GameObject> ConnectionsOnMachines()
    {
        List<GameObject> connection_gos = new List<GameObject>();

        foreach (GameObject machine_go in MachinesOnTiles())
        {
            Machine machine = machine_go.GetComponent<Machine>();
            foreach (GameObject con in machine.connections)
            {
                if (!connection_gos.Contains(con))
                {
                    connection_gos.Add(con);
                }
            }
        }

        return connection_gos;
    }

    private List<GameObject> DecorationsOnTiles()
    {
        return OnTilesByTag("decoration");
    }
}
