using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    
    public MusicList current_list = null;

    public bool play = false;
    public bool repeat = false;

    void Start()
    {
        if (play && current_list != null) { PlayCurrentMusic(); }
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

    public void ChangeTo(MusicList new_list)
    {
        ResetCurrentList();
        current_list = new_list;
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