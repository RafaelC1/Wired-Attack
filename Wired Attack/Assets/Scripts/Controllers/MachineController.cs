using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MachineController : MonoBehaviour {

    public GameController gameController = null;
    public PopUpTextController popTextController = null;
    public SoundEffectController soundController = null;
    
    public GameObject messagePreFab = null;

    public List<Machine> machines = new List<Machine>();
    public List<Connection> connections = new List<Connection>();
    
    void Start()
    {
    }

    void Update()
    {
    }

    public void StartGame(List<GameObject> machineGos, List<GameObject> connectionGos)
    {
        machines.Clear();
        PrepareAllMachines(machineGos);
        PrepareAllConnections(connectionGos);
    }

    public int NeutralMachineCount()
    {
        return machines.FindAll(machine => machine.team == TeamHelpers.Team.NEUTRAL_TEAM).Count;
    }

    public int PlayerMachineCount()
    {
        return machines.FindAll(machine => machine.team == TeamHelpers.Team.HUMAN_TEAM).Count;
    }

    public int IAMachineCount()
    {
        return machines.FindAll(machine => machine.team == TeamHelpers.Team.RED_TEAM).Count;
    }

    private void PrepareAllMachines(List<GameObject> machineGos)
    {
        foreach (GameObject machineGo in machineGos)
        {
            AddMachine(machineGo);
        }
    }

    private void PrepareAllConnections(List<GameObject> connectionGos)
    {
        foreach (GameObject connectionGo in connectionGos)
        {
            AddConnection(connectionGo);
        }
    }

    public void AddMachine(GameObject machineGo)
    {
        Machine machine = machineGo.GetComponent<Machine>();

        machine.SetTextParentAndPosition();
        machine.TurnMachineOn();
        machine.machineController = this;
        machines.Add(machine);
    }

    public void RemoveMachine(GameObject machineToRemove)
    {
        Machine machine = machineToRemove.GetComponent<Machine>();
        machines.Remove(machine);

    }

    public void AddConnection(GameObject connection)
    {
        connections.Add(connection.GetComponent<Connection>());
    }

    public void TryTransferBitsBetweenMachines(Machine to, Machine from)
    {
        string transferError = null;
        Transform currentMachineSaying = null;
        Connection connectionBetweenMachines = ConnectionBetween(to.gameObject, from.gameObject);

        if (connectionBetweenMachines != null)
        {
            if (from.CanSendBits())
            {
                if (to.CanReceiveAliedBits())
                {
                    TransferBitsBetweenMachines(connectionBetweenMachines, to, from);
                } else {
                    transferError = "cheio.";
                    currentMachineSaying = to.transform;
                }
            } else {
                transferError = "bits insuficientes.";
                currentMachineSaying = from.transform;
            }
        } else {
            transferError = "não conectadas.";
            currentMachineSaying = from.transform;
        }

        if (transferError != null)
        {
            popTextController.CreatePopText(transferError, currentMachineSaying);
        }
    }

    private void TransferBitsBetweenMachines(Connection connectionBetween, Machine to, Machine from)
    {
        GameObject message_new = Instantiate(messagePreFab);
        Message message = message_new.GetComponent<Message>();

        connectionBetween.messages.Add(message);
        message.gameController = gameController;
        message.DefineTransferSettings(from.SendBits(),
                                       connectionBetween.travelTime,
                                       from.gameObject,
                                       to.gameObject,
                                       connectionBetween);
        if (message.fromTeam == TeamHelpers.Team.HUMAN_TEAM)
            MessageSent();
    }

    public Connection ConnectionBetween(GameObject firstMachine, GameObject lastMachine)
    {
        foreach (Connection connection in connections)
        {
            if (connection.IsConnectedBetween(firstMachine, lastMachine))
            {
                return connection;
            }
        }
        return null;
    }

    public bool IsThereConnectionBetween(GameObject firstMachine, GameObject lastMachine)
    {
        return ConnectionBetween(firstMachine, lastMachine) != null;
    }

    public void DetermineTeamVictory()
    {
        if (!IsThereMoreThanOneTeamAlive() &&
            !AnyMessageStillTraveling())
                gameController.EndCurrentGame(AliveMachineTeams()[0]);
    }

    public bool IsThereMoreThanOneTeamAlive()
    {
        return AliveMachineTeams().Count > 1;
    }

    private List<TeamHelpers.Team> AliveMachineTeams()
    {
        List<TeamHelpers.Team> aliveTeams = new List<TeamHelpers.Team>();

        foreach (Machine machine in machines)
            if (!aliveTeams.Contains(machine.team))
                aliveTeams.Add(machine.team);

        return aliveTeams;
    }

    public bool AnyMessageStillTraveling()
    {
        return MessagesTraveling().Count > 0;
    }

    private List<Message> MessagesTraveling()
    {
        List<Message> messages = new List<Message>();

        foreach (Machine machine in machines)
            foreach (GameObject con in machine.connections)
                foreach (Message msg in con.GetComponent<Connection>().messages)
                    messages.Add(msg);

        return messages;
    }

    private List<TeamHelpers.Team> AliveMessageTeams()
    {
        List<TeamHelpers.Team> aliveTeams = new List<TeamHelpers.Team>();

        foreach (Machine machine in machines)
            foreach(GameObject con in machine.connections)
                foreach(Message msg in con.GetComponent<Connection>().messages)
                    if (!aliveTeams.Contains(msg.fromTeam))
                        aliveTeams.Add(msg.fromTeam);

        return aliveTeams;
    }

    public void ChangeMachineOwner()
    {
        soundController.PlaySfx("base_lost01");
    }

    public void MessageSent()
    {
        soundController.PlaySfx("package_walk01");
    }

    
}
