using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    //public string map_save_path = string.Format("{0}/Maps/", Application.streamingAssetsPath);


    public enum GameStatus
    {
        EDIT_MODE = 0,
        GAME_MODE = 1
    }

    public GameStatus current_status = GameStatus.GAME_MODE;


	void Start () {
		
	}

	void Update () {
		
	}

    public void DefineEditMode()
    {
        current_status = GameStatus.EDIT_MODE;
    }

    public void DefineGameMode()
    {
        current_status = GameStatus.GAME_MODE;
    }
}
