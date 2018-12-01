using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour {

    public GameController gameController = null;
    private float transferTime = 0f;
    private float currentTransferTime = 0f;

    private int bitAmount = 0;

    private Connection connectionIn = null;
    public GameObject from = null;
    public TeamHelpers.Team fromTeam = TeamHelpers.Team.NEUTRAL_TEAM;
    public GameObject to = null;

    void Start()
    {
        
    }

    void Update()
    {
        if (gameController.IsPaused()) return;
        MoveToDestine();

        if(ArrivedToDestine())
        {
            DelivereBits();
            connectionIn.RemoveMessage(this);
        }
    }

    public void DefineTransferSettings(int bitsToTransfer,
                               float timeToTransfer,
                               GameObject from,
                               GameObject to,
                               Connection connection)
    {
        bitAmount = bitsToTransfer;
        transferTime = timeToTransfer;

        this.from = from;
        this.fromTeam = from.GetComponent<Machine>().team;
        this.to = to;
        connectionIn = connection;

        transform.position = from.transform.position;

        DefineColor();
    }

    private void DefineColor()
    {
        Color new_color = TeamHelpers.TeamColorOf(fromTeam);
        this.GetComponent<SpriteRenderer>().color = new_color;
    }

    public bool ArrivedToDestine()
    {
        return currentTransferTime >= 1;
    }

    private void MoveToDestine()
    {
        currentTransferTime += Time.deltaTime / transferTime;
        transform.position = Vector3.Lerp(from.transform.position,
                                          to.transform.position,
                                          currentTransferTime);
    }

    public void DelivereBits()
    {
        to.GetComponent<Machine>().ReceiveBits(bitAmount, fromTeam);
    }

    private void OnDestroy()
    {
        if (from != null)
            from.GetComponent<Machine>().machineController.DetermineTeamVictory();
    }

}
