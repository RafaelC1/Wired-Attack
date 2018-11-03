using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicList : MonoBehaviour {

    public AudioClip[] musics;
    private int current_music_id = 0;

    void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    public void NextMusic()
    {
        current_music_id++;
        if (!CurrentMusicExist())
        {
            GoEndOfTheList();
        }
    }

    public void BackMusic()
    {
        current_music_id--;
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
        current_music_id = 0;
    }

    public void GoEndOfTheList()
    {
        current_music_id = MusicCount();
    }

    public AudioClip CurrentMusic()
    {
        if (musics.Length > 0 && musics.Length > current_music_id)
        {
            return musics[current_music_id];
        } else {
            return null;
        }
    }
}
