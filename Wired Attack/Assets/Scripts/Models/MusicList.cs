using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicList : MonoBehaviour {

    public Music[] musics;
    private int currentMusicId = 0;

    void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    public void NextMusic()
    {
        if (CurrentMusic().introPlayed)
        {
            CurrentMusic().Reset();
            currentMusicId++;
        } else {
            CurrentMusic().NextPart();
        }
        if (!CurrentMusicExist())
        {
            GoEndOfTheList();
        }
    }

    public void BackMusic()
    {
        currentMusicId--;
        if (!CurrentMusicExist())
        {
            GoStartOfTheList();
        }
    }

    public bool CurrentMusicExist()
    {
        return CurrentMusic() != null;
    }

    public int MusicCount()
    {
        return musics.Length - 1;
    }

    public void GoStartOfTheList()
    {
        currentMusicId = 0;
    }

    public void GoEndOfTheList()
    {
        currentMusicId = MusicCount();
    }

    public Music CurrentMusic()
    {
        if (musics.Length > 0 && musics.Length > currentMusicId)
        {
            return musics[currentMusicId];
        } else {
            return null;
        }
    }
}
