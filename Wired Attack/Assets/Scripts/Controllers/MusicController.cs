using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    
    public MusicList currentList = null;

    public bool play = false;
    public bool repeat = false;

    void Start()
    {
        if (play && currentList != null) { PlayCurrentMusic(); }
    }

    void Update()
    {
        if (!GetAudioSource().isPlaying)
        {
            currentList.NextMusic();
            if (currentList.CurrentMusicExist())
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
        GetAudioSource().clip = currentList.CurrentMusic().CurrentPart();
    }

    private void PlayCurrentMusic()
    {
        GetAudioSource().Play();
    }

    public void ChangeTo(MusicList new_list)
    {
        ResetCurrentList();
        currentList = new_list;
        UpdateCurrentMusicToClip();
        StartCurrentMusic();
    }

    private void ResetCurrentList()
    {
        if (currentList != null)
        {
            currentList.GoStartOfTheList();
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