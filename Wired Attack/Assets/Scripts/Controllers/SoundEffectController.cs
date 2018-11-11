using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour {

    public List<AudioClip> sfx_list = new List<AudioClip>();
    private IDictionary<string, AudioClip> sfx_dictionary = new Dictionary<string, AudioClip>();

    private AudioSource sound_source;

    private float current_sound_volumn = 1f;

    void Start ()
    {
        OrganizeSfxDictionary();
        sound_source = GetComponent<AudioSource>();
    }	
	
	void Update () { }

    private void OrganizeSfxDictionary()
    {
        foreach(AudioClip sfx in sfx_list)
        {
            sfx_dictionary.Add(sfx.name, sfx);
        }
    }

    public void PlaySfx(string sfx_name)
    {
        sound_source.PlayOneShot(sfx_dictionary[sfx_name], current_sound_volumn);
    }

    public void ChangeSoundSourceVolume(float new_volume)
    {
        sound_source.volume = new_volume;
    }
}
