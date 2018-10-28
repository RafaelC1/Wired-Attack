using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour {

    public AudioClip click_over_btn;

    private AudioSource sound_source;

    private float current_sound_volumn = 1f;

    void Start ()
    {
        sound_source = GetComponent<AudioSource>();
    }	
	
	void Update () { }

    public void ButtonClick()
    {
        sound_source.PlayOneShot(click_over_btn, current_sound_volumn);
    }
}
