using UnityEngine;

public class GameController : MonoBehaviour {

    public MachineController machine_controller = null;

    public GameObject player_pre_fab = null;
    public GameObject enemy_pre_fab = null;

    public GameObject players_holder = null;

    public bool pause = false;
    
    public enum GameMode
    {
        EDIT_MODE,
        PLAY_MODE,
        DEMOSTRATION
    }

    void Start () { }

	void Update () { }

    public void Pause()
    {
        pause = !pause;
    }

    public void CreateHumanPlay()
    {
        GameObject temp = Instantiate(player_pre_fab);
        temp.GetComponent<Player>().machine_controller = machine_controller;
        temp.transform.SetParent(players_holder.transform);
    }

    public void CreateIAPlayer(TeamHelpers.Team ia_team)
    {
        GameObject temp = Instantiate(enemy_pre_fab);
        IA ia = temp.GetComponent<IA>();

        temp.transform.SetParent(players_holder.transform);
        temp.name = ia_team.ToString();

        ia.machine_controller = machine_controller;
        ia.team = ia_team;
    }

    public void DestroyPlayers()
    {
        for(int i = 0; i<players_holder.transform.childCount; i++)
        {
            Destroy(players_holder.transform.GetChild(i));
        }
    }
}
