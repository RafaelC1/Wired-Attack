using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public MachineController machine_controller = null;
    public MapController map_controller = null;
    public MusicController music_controller = null;

    public List<Player> players = new List<Player>();

    public GameObject player_pre_fab = null;
    public GameObject enemy_pre_fab = null;

    public GameObject players_holder = null;

    public GameObject victory_end_menu = null;
    public GameObject defeat_end_menu = null;

    public MusicList victory_end_menu_music_list = null;
    public MusicList defeat_end_menu_music_list = null;

    public MenuController menu_controller = null;

    public FinalScore end_score = null;

    public bool pause = false;

    public float timer = 0;
    
    public enum GameMode
    {
        EDIT_MODE,
        PLAY_MODE,
        DEMOSTRATION
    }

    void Start () { }

	void Update ()
    {
        if (map_controller.current_game_mode == GameMode.PLAY_MODE && !isPaused())
            UpdateTimer();

    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    public void ResetTime()
    {
        timer = 0;
    }

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

    public void EndCurrentGame(TeamHelpers.Team team_winnner)
    {
        Player winner = players.Find(player => player.team == team_winnner);
        players.Remove(winner);
        players.Add(winner);

        Pause();

        if (winner.team == TeamHelpers.Team.HUMAN_TEAM)
        {
            menu_controller.OpenMenuByObjectCloseAll(victory_end_menu);
            music_controller.ChangeTo(victory_end_menu_music_list);
        } else {
            menu_controller.OpenMenuByObjectCloseAll(defeat_end_menu);
            music_controller.ChangeTo(defeat_end_menu_music_list);
        }

        end_score.DefineMapName(map_controller.current_map.name);
        end_score.DefineTime((int)timer);
    }
}
