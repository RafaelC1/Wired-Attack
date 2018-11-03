using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    
    public MusicList game_list;
    public MusicList menu_list;

    private MusicList current_list = null;

    public bool play = false;
    public bool repeat = false;

    void Start()
    {
        ChangeToMenuMusics();
        if (current_list == null) { ChangeToGameList(); }

        if (play) { PlayCurrentMusic(); }
    }

    void Update()
    {
        if (!GetAudioSource().isPlaying)
        {
            current_list.NextMusic();
            if (current_list.CurrentMusicExist())
            {
                UpdateCurrentMusicToClip();
                StartCurrentMusic();
            } else if (repeat) {
                ResetCurrentList();
                StartCurrentMusic();
            }
        }
    }

    private AudioSource GetAudioSource()
    {
        return GetComponent<AudioSource>();
    }

    private void UpdateCurrentMusicToClip()
    {
        GetAudioSource().Stop();
        GetAudioSource().clip = current_list.CurrentMusic();
    }

    private void PlayCurrentMusic()
    {
        GetAudioSource().Play();
    }

    public void ChangeToMenuMusics()
    {
        ResetCurrentList();
        current_list = menu_list;
        UpdateCurrentMusicToClip();
        StartCurrentMusic();
    }

    public void ChangeToGameList()
    {
        ResetCurrentList();
        current_list = game_list;
        UpdateCurrentMusicToClip();
        StartCurrentMusic();
    }

    private void ResetCurrentList()
    {
        if (current_list != null)
        {
            current_list.GoStartOfTheList();
        }
    }

    public void StartCurrentMusic()
    {
        GetAudioSource().Play();
    }

    public void ChangeSoundSourceVolume(float new_volume)
    {
        GetAudioSource().volume = new_volume;
    }
}