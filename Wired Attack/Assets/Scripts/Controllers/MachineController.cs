using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MachineController : MonoBehaviour {

    public GameController game_controller = null;
    public PopUpTextController pop_text_controller = null;

    public GameObject connection_pre_fab = null;
    public GameObject message_pre_fab = null;

    public List<Machine> machines = new List<Machine>();
    public List<Connection> connections = new List<Connection>();
    
    void Start()
    {
    }

    void Update()
    {
    }

    public void StartGame(List<GameObject> machine_gos, List<GameObject> connection_gos)
    {
        PrepareAllMachines(machine_gos);
        PrepareAllConnections(connection_gos);
    }

    public void DestroyAllMachines()
    {
        foreach(Machine machine in machines)
        {
            Destroy(machine.gameObject);
        }
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

    private void PrepareAllMachines(List<GameObject> machine_gos)
    {
        foreach (GameObject machine_go in machine_gos)
        {
            AddMachine(machine_go);
        }
    }

    private void PrepareAllConnections(List<GameObject> connection_gos)
    {
        foreach (GameObject connection_go in connection_gos)
        {
            AddConnection(connection_go);
        }
    }

    public void AddMachine(GameObject machine_go)
    {
        Machine machine = machine_go.GetComponent<Machine>();

        machine.SetTextParentAndPosition();
        machine.TurnMachineOn();
        machine.controller = this;
        machines.Add(machine);
    }

    public void RemoveMachine(GameObject machine_to_remove)
    {
        Machine machine = machine_to_remove.GetComponent<Machine>();
        machines.Remove(machine);

    }

    public void AddConnection(GameObject connection)
    {
        connections.Add(connection.GetComponent<Connection>());
    }

    public void TryTransferBitsBetweenMachines(Machine to, Machine from)
    {
        string transfer_error = null;
        Transform current_machine_saying = null;
        Connection connection_between_machines = ConnectionBetween(to.gameObject, from.gameObject);

        if (connection_between_machines != null)
        {
            if (from.CanSendBits())
            {
                if (to.CanReceiveAliedBits())
                {
                    TransferBitsBetweenMachines(connection_between_machines, to, from);
                } else {
                    transfer_error = "cheio.";
                    current_machine_saying = to.transform;
                }
            } else {
                transfer_error = "bits insuficientes.";
                current_machine_saying = from.transform;
            }
        } else {
            transfer_error = "não conectadas.";
            current_machine_saying = from.transform;
        }

        if (transfer_error != null)
        {
            pop_text_controller.CreatePopText(transfer_error, current_machine_saying);
        }
    }

    private void TransferBitsBetweenMachines(Connection connection_between, Machine to, Machine from)
    {
        GameObject message_new = Instantiate(message_pre_fab);
        Message message = message_new.GetComponent<Message>();

        connection_between.messages.Add(message);
        message.DefineTransferSettings(from.SendBits(),
                                       connection_between.travel_time,
                                       from.gameObject,
                                       to.gameObject,
                                       connection_between);
    }

    public Connection ConnectionBetween(GameObject first_machine, GameObject last_machine)
    {
        foreach (Connection connection in connections)
        {
            if (connection.IsConnectedBetween(first_machine, last_machine))
            {
                return connection;
            }
        }
        return null;
    }

    public bool IsThereConnectionBetween(GameObject first_machine, GameObject last_machine)
    {
        return ConnectionBetween(first_machine, last_machine) != null;
    }
}
