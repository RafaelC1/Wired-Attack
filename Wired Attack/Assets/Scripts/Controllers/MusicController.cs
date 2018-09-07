using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public AudioClip click_over_btn;
    public AudioClip[] musics;

    private AudioSource source;

    private float current_sound_volumn = 1f;
    private float current_music_volumn = 1f;

    void Start ()
    {
        source = GetComponent<AudioSource>();
    }	
	
	void Update () { }

    public void ButtonClick()
    {
        source.PlayOneShot(click_over_btn, current_sound_volumn);
    }
    
}
