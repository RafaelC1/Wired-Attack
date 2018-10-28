using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public AudioClip[] music;
    private int current_music_id = -1;

    private AudioSource sound_source;

    public bool play = false;
    public bool repeat = false;
    private float current_music_volumn = 1f;

    void Start()
    {
        sound_source = GetComponent<AudioSource>();
        ResetList();
        if (play) { StartCurrentMusic(); }
    }

    void Update()
    {
        if (!sound_source.isPlaying && play)
        {
            NextMusic();
            StartCurrentMusic();
        }
    }

    public void StartPlayingList()
    {
        ResetList();
        play = true;
    }

    public void StartCurrentMusic()
    {
        sound_source.Play();
    }

    private void UpdateCurrentMusicToClip()
    {
        sound_source.clip = music[current_music_id];
    }

    public void NextMusic()
    {
        current_music_id++;
        if (!CurrentMusicIdExist()){ ResetList(); }
        UpdateCurrentMusicToClip();
    }

    public void BackMusic()
    {
        current_music_id--;
        if (!CurrentMusicIdExist()) { current_music_id = MusicCount(); }
        UpdateCurrentMusicToClip();
    }

    public void ResetList()
    {
        if (!repeat) { play = false; }
        current_music_id = 0;
    }

    public int MusicCount()
    {
        return music.Length - 1;
    }

    private bool CurrentMusicIdExist()
    {
        return current_music_id >= 0 && current_music_id < music.Length - 1;
    }

    private void OnEnable()
    {
        sound_source.Pause();
        current_music_id = 0;
    }
}