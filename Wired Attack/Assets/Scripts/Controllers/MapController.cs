using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Dynamic;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour {

    #region variables

    public GameObject machine_controller = null;
    public GameController game_controller = null;

    public GameObject floor_pre_fab = null;

    public MenuController editor_mode_menus = null;
    public GameObject floor_editor_menu = null;
    public GameObject machine_editor_menu = null;
    //public GameObject decoration_editor_menu = null;

    public List<GameObject> wire_pre_fabs = new List<GameObject>();
    public List<GameObject> machines_pre_fabs = new List<GameObject>();

    private List<GameObject> tiles = new List<GameObject>();

    public GameObject machine_parent = null;
    public GameObject connection_parent = null;
    public GameObject canvas = null;

    private GameObject selected_floor = null;

    private Vector3 tile_size;

    public string map_to_load_name = "";

    public float gap_size = 0;
    public int height = 7;
    public int width = 5;

    #endregion

    void Start ()
    {
    }

    void Update()
    {
        if (game_controller.current_status == GameController.GameStatus.EDIT_MODE)
        {
            DetectClickOnTiles();
        }
    }
    
    public void CreateMap()
    {
        CreateTiles();
        DesactiveAllMenus();

        if (map_to_load_name != "")
        {
            LoadMapByName(map_to_load_name);
        }

        if (game_controller.current_status == GameController.GameStatus.GAME_MODE)
        {
            machine_controller.GetComponent<MachineController>().StartGame();
        }
    }

    public void DefineNextMap(string next_map)
    {
        map_to_load_name = next_map;
    }

    private void CreateTiles()
    {
        tile_size = floor_pre_fab.GetComponent<Renderer>().bounds.size;

        float total_width = tile_size.x * width;
        float total_height = tile_size.y * height;

        Vector3 start_pos = new Vector3();

        start_pos.x = -((total_width / 2) - tile_size.x / 2);
        start_pos.y = +((total_height / 2) - tile_size.x / 2);

        Vector3 current_pos = start_pos;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject temp = Instantiate(floor_pre_fab);
                Floor temp_floor = temp.GetComponent<Floor>();

                temp.transform.SetParent(this.transform);
                temp.transform.position = current_pos;
                current_pos.x += tile_size.x;

                temp.transform.name = string.Format("Tile{0}x{1}", j+1, i+1);
                temp_floor.controller = this.gameObject;
                temp_floor.id = tiles.Count;

                if (game_controller.current_status == GameController.GameStatus.GAME_MODE)
                {
                    temp.GetComponent<BoxCollider2D>().enabled = false;
                }

                tiles.Add(temp);
            }
            current_pos.x = start_pos.x;
            current_pos.y -= tile_size.y;
        }        
    }

    private void DetectClickOnTiles()
    {
        if (Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject())
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
                        Floor current_floor = selected_floor.GetComponent<Floor>();
                        Floor another_floor = hit.transform.GetComponent<Floor>();

                        if (current_floor.IsHoldingSomething() &&
                            another_floor.IsHoldingSomething() &&
                            !machine_controller.GetComponent<MachineController>()
                                               .IsThereConnectionBetween(current_floor.object_holded.GetComponent<Machine>(),
                                                                         another_floor.object_holded.GetComponent<Machine>()))
                        {
                            CreateConnectionBetweenMachinesOn(selected_floor, hit.transform.gameObject, WirePreFabByType("optical"));
                            DeselectCurrentFloor();
                            return;
                        }

                        DeselectCurrentFloor();
                    }

                    SelectFloor(hit.transform.gameObject);

                    if (IsCurrentSelectedFloorHoldingSomething())
                    {
                        ActiveMachineEditorMenu(true);
                    }
                    else if (!IsCurrentSelectedFloorHoldingSomething())
                    {
                        ActiveFloorEditorMenu(true);
                    }

                } else {
                    DeselectCurrentFloor();
                }
            }
        }
    }

    private void SelectFloor(GameObject floor)
    {
        selected_floor = floor;
        selected_floor.GetComponent<Floor>().ActiveBackGround(true);
    }

    private void DeselectCurrentFloor()
    {
        selected_floor.GetComponent<Floor>().ActiveBackGround(false);
        selected_floor = null;
    }

    public void PlaceNewMachineOnSelectedTile(string machine_model)
    {
        GameObject machine_go = Instantiate(MachinePreFabByModel(machine_model));
        Machine machine = machine_go.GetComponent<Machine>();

        machine.id = Machines().Count + 1;
        machine_go.transform.name = machine.model + machine.id.ToString();

        GiveToSelectedFloor(machine_go);
    }

    private void GiveToSelectedFloor(GameObject object_to_hold)
    {
        selected_floor.GetComponent<Floor>().ReceiveObjectToHold(object_to_hold);

        object_to_hold.transform.SetParent(machine_parent.transform);

        Machine machine = object_to_hold.GetComponent<Machine>();
        machine.canvas = canvas;
        machine.SetTextParentAndPosition();

        DeselectCurrentFloor();
    }

    public void RemoveFromSelectedFloor()
    {
        Floor floor = selected_floor.GetComponent<Floor>();
        floor.RemoveHoldedObject();
        DeselectCurrentFloor();
    }

    public void CreateConnectionBetweenMachinesOn(GameObject first_floor, GameObject secound_floor, GameObject wire_pre_fab)
    {
        GameObject first_machine = first_floor.GetComponent<Floor>().object_holded;
        GameObject secound_machine = secound_floor.GetComponent<Floor>().object_holded;

        if (first_machine.GetComponent<Machine>().CanHaveMoreConnections() &&
            secound_machine.GetComponent<Machine>().CanHaveMoreConnections())
        {
            GameObject wire_go = Instantiate(wire_pre_fab);
            Wire wire = wire_go.GetComponent<Wire>();

            wire.connection_points[0] = first_machine;
            wire.connection_points[1] = secound_machine;
            
            wire.id = Connections().Count;

            wire_go.name = wire.type + wire.id.ToString();

            wire_go.transform.SetParent(connection_parent.transform);
        } else {
            game_controller.GetComponent<PopUpTextController>().CreatePopText("connections limit", first_machine.transform);
        }
    }

    private bool IsCurrentSelectedFloorHoldingSomething()
    {
        return selected_floor.GetComponent<Floor>().IsHoldingSomething();
    }
    
    public void SaveCurrentMap()
    {
        DataController data_controller = new DataController();
        List<string> data_prepared_to_save = PrepareCurrentMapBeforeSave();

        data_controller.SaveMap(data_prepared_to_save, map_to_load_name);
    }

    public List<string> PrepareCurrentMapBeforeSave()
    {
        List<string> lines_of_serialized_model = new List<string>();

        lines_of_serialized_model.Add(DataController.START_OF_MACHINE_SERIALIZED);

        foreach (GameObject machine in Machines())
        {
            MachineSerialized machine_serialized = new MachineSerialized(machine.GetComponent<Machine>());
            string json_format_of_serialized_model = JsonUtility.ToJson(machine_serialized);

            lines_of_serialized_model.Add(json_format_of_serialized_model);
        }

        lines_of_serialized_model.Add(DataController.START_OF_CONNECTION_SERIALIZED);

        foreach (GameObject con in Connections())
        {
            WireSerialized connection_serialized = new WireSerialized(con.GetComponent<Wire>());
            string json_format_of_serialized_model = JsonUtility.ToJson(connection_serialized);

            lines_of_serialized_model.Add(json_format_of_serialized_model);
        }

        return lines_of_serialized_model;
    }

    public void LoadMapByName(string map_name)
    {
        DataController data_controller = new DataController();
        data_controller.LoadMap(map_name);

        List<MachineSerialized> serialized_machines = data_controller.map_file_machines;
        List<WireSerialized> serialized_connections = data_controller.map_file_connections;

        foreach (MachineSerialized machine_s in serialized_machines)
        {
            GameObject machine_go = Instantiate(MachinePreFabByModel(machine_s.machine_model));

            Machine machine = machine_go.GetComponent<Machine>();
            machine.id = machine_s.id;
            machine.team_id = machine_s.team_id;
            machine.model = machine_s.machine_model;

            selected_floor = tiles[machine_s.floor_id];

            GiveToSelectedFloor(machine_go);
        }

        foreach(WireSerialized con_s in serialized_connections)
        {
            GameObject first_machine_point = Machines().Find(machine => machine.GetComponent<Machine>().id == con_s.connection_ids[0]);
            GameObject secound_machine_point = Machines().Find(machine => machine.GetComponent<Machine>().id == con_s.connection_ids[1]);

            GameObject first_floor = first_machine_point.GetComponent<Machine>().current_floor.transform.gameObject;
            GameObject secound_floor = secound_machine_point.GetComponent<Machine>().current_floor.transform.gameObject;

            CreateConnectionBetweenMachinesOn(first_floor, secound_floor, WirePreFabByType(con_s.wire_type));
        }
    }

    private void DesactiveAllMenus()
    {
        ActiveFloorEditorMenu(false);
        ActiveMachineEditorMenu(false);
    }

    private void ActiveFloorEditorMenu(bool activate)
    {
        if (activate)
        {
            editor_mode_menus.OpenMenuByObject(floor_editor_menu);
        } else {
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

    private GameObject WirePreFabByType(string wire_name)
    {
        return wire_pre_fabs.Find(wire => wire.GetComponent<Wire>().type == wire_name);
    }

    private GameObject MachinePreFabByModel(string machine_model)
    {
        return machines_pre_fabs.Find(machine => machine.GetComponent<Machine>().model == machine_model);
    }

    private GameObject FloorById(int tile_id)
    {
        return tiles.Find(tile => tile.GetComponent<Floor>().id == tile_id);
    }

    private List<GameObject> Machines()
    {
        List<GameObject> machine_gos = new List<GameObject>();

        foreach(GameObject floor in tiles)
        {
            GameObject holded_object = floor.GetComponent<Floor>().object_holded;
            if (holded_object != null && holded_object.tag == "machine")
            {
                machine_gos.Add(holded_object);
            }
        }

        return machine_gos;
    }

    private List<GameObject> Connections()
    {
        List<GameObject> wire_gos = new List<GameObject>();

        foreach(GameObject machine_go in Machines())
        {
            Machine machine = machine_go.GetComponent<Machine>();
            foreach(GameObject con in machine.connections)
            {
                wire_gos.Add(con);
            }
        }

        return wire_gos;
    }
}
