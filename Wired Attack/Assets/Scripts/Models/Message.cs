using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour {

    private float transfer_time = 0f;
    private float current_transfer_time = 0f;

    private int bit_amount = 0;

    private Connection connection_in = null;
    public GameObject from = null;
    public TeamHelpers.Team from_team = TeamHelpers.Team.NEUTRAL_TEAM;
    public GameObject to = null;

    void Start()
    {
        
    }

    void Update()
    {
        MoveToDestine();

        if(ArrivedToDestine())
        {
            DelivereBits();
            connection_in.RemoveMessage(this);
        }
    }

    public void DefineTransferSettings(int bits_to_transfer,
                               float time_to_transfer,
                               GameObject from,
                               GameObject to,
                               Connection connection)
    {
        bit_amount = bits_to_transfer;
        transfer_time = time_to_transfer;

        this.from = from;
        this.from_team = from.GetComponent<Machine>().team;
        this.to = to;
        connection_in = connection;

        transform.position = from.transform.position;
    }

    public bool ArrivedToDestine()
    {
        return current_transfer_time >= 1;
    }

    private void MoveToDestine()
    {
        current_transfer_time += Time.deltaTime / transfer_time;
        transform.position = Vector3.Lerp(from.transform.position,
                                          to.transform.position,
                                          current_transfer_time);
    }

    public void DelivereBits()
    {
        to.GetComponent<Machine>().ReceiveBits(bit_amount, from_team);
    }

}
