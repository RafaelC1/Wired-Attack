using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Dynamic;

public class Machine : Holdable {

    #region variables

    public TeamHelpers.Team team = TeamHelpers.Team.NEUTRAL_TEAM;
    public string model = "";

    public MachineController machineController;
    public GameController gameController = null;
    public List<GameObject> connections = new List<GameObject>();

    public GameObject textParent = null;

    public GameObject background = null;

    public GameObject text_holder;
    public Text bits_label;

    public int maxConnections = 1;

    public int maxBitStorage = 10;
    public int currentStoredBits = 0;

    private int bitPerProcess = 1;

    public float processTime = 1;
    public float currentProcessTime = 0;

    public int bitDefense = 1;
    //public int bit_attack = 1;

    public bool produceBit = true;

    public bool actived;

    #endregion

    void Start()
    {
        UpdateExternalInformations();
        ChangeColor();
        ActiveBackGround(false);
    }

    public void SetTextParentAndPosition()
    {
        bits_label.gameObject.SetActive(true);
        bits_label.transform.SetParent(textParent.transform);
        bits_label.transform.position = text_holder.transform.position;
        bits_label.transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        if (gameController.IsPaused()) return;
        if (actived && !IsNeutral())
        {
            Process();
        }
    }

    private void Process()
    {
        if (!IsStorageFull() && produceBit)
        {
            currentProcessTime += Time.deltaTime;

            if (currentProcessTime >= processTime)
            {
                StoreBits(bitPerProcess);
                currentProcessTime = 0;
            }
        }
    }

    public void AddConnection(GameObject connection)
    {
        connections.Add(connection);
    }

    public List<Machine> ConnectedMachines()
    {
        List<Machine> points = new List<Machine>();
        foreach (GameObject connectionGo in connections)
            foreach (GameObject machineGo in connectionGo.GetComponent<Connection>().connectionsPoints)
            {
                Machine machine = machineGo.GetComponent<Machine>();
                if (machine != this)
                    points.Add(machine);
            }

        return points;
    }

    public bool CanHaveMoreConnections()
    {
        return connections.Count <= maxConnections;
    }

    public bool CanSendBits()
    {
        return currentStoredBits > 0;
    }

    public bool CanReceiveAliedBits()
    {
        //return !(current_stored_bits == max_bit_storage);
        return true;
    }

    private void UpdateExternalInformations()
    {
        bits_label.text = AmountOfBitsFormatted();
    }

    public void ChangeOwner(TeamHelpers.Team new_team)
    {
        team = new_team;
        ChangeColor();
        if (machineController != null)
        {
            machineController.DetermineTeamVictory();
            machineController.ChangeMachineOwner();
        }
    }

    public bool IsNeutral()
    {
        return team == TeamHelpers.Team.NEUTRAL_TEAM;
    }

    private void ChangeColor()
    {
        Color new_color = TeamHelpers.TeamColorOf(team);
        this.GetComponent<SpriteRenderer>().color = new_color;
    }

    private void StoreBits(int amountToStorage)
    {
        currentStoredBits += amountToStorage;

        if (currentStoredBits > maxBitStorage)
        {
            currentStoredBits = maxBitStorage;
        }

        UpdateExternalInformations();
    }

    public void ReceiveBits(int receivedBits, TeamHelpers.Team senderTeam)
    {
        if (senderTeam == team)
        {
            StoreBits(receivedBits);
        }
        else
        {
            receivedBits /= bitDefense;

            StoreBits(-receivedBits);

            if (currentStoredBits < 0)
            {
                int leftover_bits = -currentStoredBits;
                currentStoredBits = 0;
                StoreBits(leftover_bits);
                ChangeOwner(senderTeam);
            }
        }
    }

    public List<Message> AllMyMessagesOnMyWay()
    {
        List<Message> messageOnWay = new List<Message>();
        foreach(GameObject connectionGo in connections)
        {
            messageOnWay.AddRange(connectionGo.GetComponent<Connection>().AllMessagesOnWayTo(this));
        }
 
        return messageOnWay;
    }

    public int SendBits()
    {
        int bitsToSend = 0;

        if (currentStoredBits % 2 > 0)
        {
            if (currentStoredBits > 1)
            {
                bitsToSend = (currentStoredBits - 1) / 2;
            } else {
                bitsToSend = currentStoredBits;
            }
        } else {
            bitsToSend = currentStoredBits / 2;
        }

        currentStoredBits -= bitsToSend;

        UpdateExternalInformations();

        return bitsToSend;
    }

    public string AmountOfBitsFormatted()
    {
        return string.Format("{0}/{1}", currentStoredBits, maxBitStorage);
    }
    
    public void ActiveBackGround(bool activate)
    {
        background.GetComponent<SpriteRenderer>().enabled = activate;
    }

    public bool IsStorageFull()
    {
        return currentStoredBits >= maxBitStorage;
    }

    public bool IsStorageEmpty()
    {
        return currentStoredBits <= 0;
    }

    public void TurnMachineOn()
    {
        actived = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void TurnMachineOff()
    {
        actived = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnDestroy()
    {
        List<GameObject> connections = this.connections;

        foreach (GameObject con in connections)
        {
            con.GetComponent<Connection>().connectionsPoints.Remove(this.gameObject);
            Destroy(con);
        }
        if (bits_label != null)
        {
            Destroy(bits_label.gameObject);
        }
    }

    public void RemoveConnection(GameObject con_to_remove)
    {
        connections.Remove(con_to_remove);
    }
    
}
