using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Dynamic;

public class Machine : MonoBehaviour {

    #region variables

    public int id;
    public int team_id = 0;
    public string model =  "regular_machine";
    public string description = "machine";

    public MachineController controller;
    public List<GameObject> connections = new List<GameObject>();

    public GameObject canvas = null;

    public GameObject background = null;

    public GameObject text_holder;
    public Text bits_label;
    public Floor current_floor = null;

    public int max_connections = 1;

    public int max_bit_storage = 10;
    public int current_bit_stored = 0;

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
        bits_label.transform.SetParent(canvas.transform);
        bits_label.transform.position = text_holder.transform.position;
        bits_label.transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        if (actived)
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

    public bool CanHaveMoreConnections()
    {
        return connections.Count < max_connections;
    }

    private void UpdateExternalInformations()
    {
        bits_label.text = AmountOfBitsFormatted();
    }

    private void ChangeOwner(int new_team)
    {
        team_id = new_team;
        ChangeColor();
    }

    private void ChangeColor()
    {
        Color new_color = TeamHelpers.TeamColorOf(team_id);
        this.GetComponent<SpriteRenderer>().color = new_color;
    }

    private void StoreBits(int amount_to_store)
    {
        current_bit_stored += amount_to_store;

        if (current_bit_stored > max_bit_storage)
        {
            current_bit_stored = max_bit_storage;
        }

        UpdateExternalInformations();
    }

    public void ReceiveBits(int received_bit, Machine sender)
    {
        if (sender.team_id == team_id)
        {
            StoreBits(received_bit);
        }
        else
        {
            received_bit /= bit_defense;

            StoreBits(-received_bit);

            if (current_bit_stored < 0)
            {
                int leftover_bits = -current_bit_stored;
                current_bit_stored = 0;
                StoreBits(leftover_bits);
                ChangeOwner(sender.team_id);
            }
        }
    }

    public int SendBits()
    {
        int bits_to_send = 0;

        if (current_bit_stored % 2 > 0)
        {
            bits_to_send = (current_bit_stored - 1) / 2;
        } else {
            bits_to_send = current_bit_stored / 2;
        }

        current_bit_stored -= bits_to_send;

        UpdateExternalInformations();

        return bits_to_send;
    }

    public string AmountOfBitsFormatted()
    {
        return string.Format("{0}/{1}", current_bit_stored, max_bit_storage);
    }
    
    public void ActiveBackGround(bool activate)
    {
        background.GetComponent<SpriteRenderer>().enabled = activate;
    }

    private bool IsStorageFull()
    {
        return current_bit_stored >= max_bit_storage;
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
            con.GetComponent<Wire>().connection_points.Remove(this.gameObject);
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
