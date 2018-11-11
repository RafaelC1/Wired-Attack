using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public MachineController machine_controller = null;
    public List<Player> players = new List<Player>();

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

    public void PauseResume()
    {
        pause = !pause;
    }

    public void Pause()
    {
        pause = true;
    }

    public void Resume()
    {
        pause = false;
    }

    public bool isPaused()
    {
        return pause;
    }

    public void CreateHumanPlay()
    {
        GameObject temp = Instantiate(player_pre_fab);
        Player player = temp.GetComponent<Player>();

        player.machine_controller = machine_controller;
        player.game_controller = this;

        temp.transform.SetParent(players_holder.transform);

        players.Add(player);
    }

    public void CreateIAPlayer(TeamHelpers.Team ia_team)
    {
        GameObject temp = Instantiate(enemy_pre_fab);
        IA ia = temp.GetComponent<IA>();

        temp.transform.SetParent(players_holder.transform);
        temp.name = ia_team.ToString();

        ia.machine_controller = machine_controller;
        ia.team = ia_team;
        ia.game_controller = GetComponent<GameController>();

        players.Add(ia);
    }

    public void RemoveAllPlayers()
    {
        foreach(Player pl in players)
        {
            Destroy(pl.gameObject);
        }

        players.Clear();
    }

    public void DestroyPlayers()
    {
        for(int i = 0; i<players_holder.transform.childCount; i++)
        {
            Destroy(players_holder.transform.GetChild(i));
        }
    }
}
