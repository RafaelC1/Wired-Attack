using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MachineController : MonoBehaviour {
    
    public GameObject wire_pre_fab = null;

    public List<Machine> machines = new List<Machine>();
    public List<Wire> connections = new List<Wire>();
    
    public Machine sender_machine = null;
    public Machine receiver_machine = null;
    
    public GameController game_controller = null;

    void Start()
    {
    }

    void Update()
    {
        if (game_controller.current_status == GameController.GameStatus.GAME_MODE)
        {
            DetectClickOnMachines();
        }
    }

    public void StartGame()
    {
        FindAllMachinesOnGame();
        FindAllConnectionsOnGame();
    }

    private void FindAllMachinesOnGame()
    {
        GameObject[] machines_game_objects = GameObject.FindGameObjectsWithTag("machine");

        foreach (GameObject machine_game_object in machines_game_objects)
        {
            AddMachine(machine_game_object);
            machine_game_object.GetComponent<Machine>().TurnMachineOn();
        }
    }

    private void FindAllConnectionsOnGame()
    {
        GameObject[] connection_game_objects = GameObject.FindGameObjectsWithTag("wire");

        foreach (GameObject connection_game_object in connection_game_objects)
        {
            connections.Add(connection_game_object.GetComponent<Wire>());
        }
    }

    public void AddMachine(GameObject machine)
    {
        machines.Add(machine.GetComponent<Machine>());
        machines[machines.Count - 1].controller = this;
    }

    public void RemoveMachine(GameObject machine_to_remove)
    {
        Machine machine = machine_to_remove.GetComponent<Machine>();
        machines.Remove(machine);

    }

    public void AddConnection(GameObject connection)
    {
        connections.Add(connection.GetComponent<Wire>());
    }

    private void DetectClickOnMachines()
    {
        if (Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            Debug.Log(1);
            if (hit && hit.transform.tag == "machine")
            {
                SelectMachine(hit.transform.GetComponent<Machine>());
            } else {
                DeselectAllMachines();
            }
        }
    }

    private void TransferBitsBetweenMachines(Wire connection_between, Machine to, Machine from)
    {
        int bits_to_send = from.SendBits();

        connection_between.SendBitsBetween(to, from, bits_to_send);
    }

    public void SelectMachine(Machine selected_machine)
    {
        if (sender_machine == null)
        {
            if (selected_machine.team_id == TeamHelpers.HUMAN_TEAM)
            {
                sender_machine = selected_machine;
                sender_machine.ActiveBackGround(true);
            }
        } else if (receiver_machine == null)
        {
            Wire connection_between_machines = ConnectionBetween(sender_machine, selected_machine);
            receiver_machine = selected_machine;
            if (connection_between_machines != null)
            {
                TransferBitsBetweenMachines(connection_between_machines, selected_machine, sender_machine);
            }
            else {
                Debug.Log("não conectadas");
            }

            DeselectAllMachines();
        }
    }

    public void DeselectAllMachines()
    {
        if (sender_machine != null)
        {
            sender_machine.ActiveBackGround(false);
            sender_machine = null;
        }

        if (receiver_machine != null)
        {
            receiver_machine.ActiveBackGround(false);
            receiver_machine = null;
        }
    }

    public Wire ConnectionBetween(Machine first_machine, Machine last_machine)
    {
        foreach (Wire wire in connections)
        {
            if (wire.IsConnectedBetween(first_machine, last_machine))
            {
                return wire;
            }
        }
        return null;
    }

    public bool IsThereConnectionBetween(Machine first_machine, Machine last_machine)
    {
        return ConnectionBetween(first_machine, last_machine) != null;
    }

    public bool IsAlreadySelectedAMachine()
    {
        return sender_machine != null;
    }

}
