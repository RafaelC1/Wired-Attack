using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Dynamic;

public class Machine : Holdable {

    #region variables

    public TeamHelpers.Team team = TeamHelpers.Team.NEUTRAL_TEAM;
    public string model = "regular_machine";
    public string description = "machine";

    public MachineController controller;
    public List<GameObject> connections = new List<GameObject>();

    public GameObject canvas = null;

    public GameObject background = null;

    public GameObject text_holder;
    public Text bits_label;

    public int max_connections = 1;

    public int max_bit_storage = 10;
    public int current_stored_bits = 0;

    private int bit_per_process = 1;

    public float process_time = 1;
    public float current_process_time = 0;

    public int bit_defense = 1;
    //public int bit_attack = 1;

    public bool produce_bit = true;

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
        bits_label.transform.SetParent(canvas.transform);
        bits_label.transform.position = text_holder.transform.position;
        bits_label.transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        if (actived && !IsNeutral())
        {
            Process();
        }
    }

    private void Process()
    {
        if (!IsStorageFull() && produce_bit)
        {
            current_process_time += Time.deltaTime;

            if (current_process_time >= process_time)
            {
                StoreBits(bit_per_process);
                current_process_time = 0;
            }
        }
    }

    public void AddConnection(GameObject connection)
    {
        connections.Add(connection);
    }

    public bool IsConnectedTo(Machine machine_to_check)
    {
        bool is_connected_to = false;

        return is_connected_to;
    }

    public List<Machine> ConnectedMachines()
    {
        List<Machine> points = new List<Machine>();
        foreach (GameObject con_go in connections)
            foreach (GameObject machine_go in con_go.GetComponent<Connection>().connection_points)
            {
                Machine machine = machine_go.GetComponent<Machine>();
                if (machine != this)
                    points.Add(machine);
            }

        return points;
    }

    public bool CanHaveMoreConnections()
    {
        return connections.Count < max_connections;
    }

    public bool CanSendBits()
    {
        return current_stored_bits > 0;
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

    private void StoreBits(int amount_to_store)
    {
        current_stored_bits += amount_to_store;

        if (current_stored_bits > max_bit_storage)
        {
            current_stored_bits = max_bit_storage;
        }

        UpdateExternalInformations();
    }

    public void ReceiveBits(int received_bit, TeamHelpers.Team sender_team)
    {
        if (sender_team == team)
        {
            StoreBits(received_bit);
        }
        else
        {
            received_bit /= bit_defense;

            StoreBits(-received_bit);

            if (current_stored_bits < 0)
            {
                int leftover_bits = -current_stored_bits;
                current_stored_bits = 0;
                StoreBits(leftover_bits);
                ChangeOwner(sender_team);
            }
        }
    }

    public int SendBits()
    {
        int bits_to_send = 0;

        if (current_stored_bits % 2 > 0)
        {
            if (current_stored_bits > 1)
            {
                bits_to_send = (current_stored_bits - 1) / 2;
            } else {
                bits_to_send = current_stored_bits;
            }
        } else {
            bits_to_send = current_stored_bits / 2;
        }

        current_stored_bits -= bits_to_send;

        UpdateExternalInformations();

        return bits_to_send;
    }

    public string AmountOfBitsFormatted()
    {
        return string.Format("{0}/{1}", current_stored_bits, max_bit_storage);
    }
    
    public void ActiveBackGround(bool activate)
    {
        background.GetComponent<SpriteRenderer>().enabled = activate;
    }

    public bool IsStorageFull()
    {
        return current_stored_bits >= max_bit_storage;
    }

    public bool IsStorageEmpty()
    {
        return current_stored_bits == 0;
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
            con.GetComponent<Connection>().connection_points.Remove(this.gameObject);
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
