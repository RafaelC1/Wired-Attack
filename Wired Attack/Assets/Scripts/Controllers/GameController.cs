using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public MachineController machineController = null;
    public MapController mapController = null;
    public MusicController musicController = null;

    public List<Player> players = new List<Player>();

    public GameObject playerPreFab = null;
    public GameObject enemyPreFab = null;

    public GameObject playersHolder = null;

    public GameObject victoryEndMenu = null;
    public GameObject defeatEndMenu = null;

    public MusicList victoryEndMenuMusicList = null;
    public MusicList defeatEndMenuMusicList = null;

    public MenuController menuController = null;

    public FinalScore endScore = null;

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
        if (mapController.currentGameMode == GameMode.PLAY_MODE && !IsPaused())
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

    public bool IsPaused()
    {
        return pause;
    }

    public void CreateHumanPlay()
    {
        GameObject temp = Instantiate(playerPreFab);
        Player player = temp.GetComponent<Player>();

        player.machineController = machineController;
        player.gameController = this;

        temp.transform.SetParent(playersHolder.transform);

        players.Add(player);
    }

    public void CreateIAPlayer(TeamHelpers.Team ia_team)
    {
        GameObject temp = Instantiate(enemyPreFab);
        IA ia = temp.GetComponent<IA>();

        temp.transform.SetParent(playersHolder.transform);
        temp.name = ia_team.ToString();

        ia.machineController = machineController;
        ia.team = ia_team;
        ia.gameController = GetComponent<GameController>();

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
            menuController.OpenMenuByObjectCloseAll(victoryEndMenu);
            musicController.ChangeTo(victoryEndMenuMusicList);
        } else {
            menuController.OpenMenuByObjectCloseAll(defeatEndMenu);
            musicController.ChangeTo(defeatEndMenuMusicList);
        }

        endScore.DefineMapName(mapController.currentMap.name);
        endScore.DefineTime((int)timer);
    }
}
