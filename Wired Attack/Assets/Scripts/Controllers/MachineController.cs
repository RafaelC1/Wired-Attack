using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MachineController : MonoBehaviour {

    public GameController game_controller = null;
    public PopUpTextController pop_text_controller = null;

    public GameObject wire_pre_fab = null;

    public List<Machine> machines = new List<Machine>();
    public List<Wire> connections = new List<Wire>();
    

    void Start()
    {
    }

    void Update()
    {
    }

    public void StartGame()
    {
        FindAllMachinesOnGame();
        FindAllConnectionsOnGame();
    }

    public int NeutralMachineCount()
    {
        return machines.FindAll(machine => machine.team_id == TeamHelpers.NEUTRAL_TEAM).Count;
    }

    public int PlayerMachineCount()
    {
        return machines.FindAll(machine => machine.team_id == TeamHelpers.HUMAN_TEAM).Count;
    }

    public int IAMachineCount()
    {
        return machines.FindAll(machine => machine.team_id == TeamHelpers.IA_TEAM).Count;
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

    public void TryTransferBitsBetweenMachines(Machine to, Machine from)
    {
        Wire connection_between_machines = ConnectionBetween(to, from);
        if (connection_between_machines != null)
        {
            TransferBitsBetweenMachines(connection_between_machines, to, from);
        } else {
            pop_text_controller.CreatePopText("sem conexão", from.transform);
        }
    }

    private void TransferBitsBetweenMachines(Wire connection_between, Machine to, Machine from)
    {
        int bits_to_send = from.SendBits();

        connection_between.SendBitsBetween(to, from, bits_to_send);
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
}
