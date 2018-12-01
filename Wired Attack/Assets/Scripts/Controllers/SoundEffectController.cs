using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour {

    public List<AudioClip> sfxList = new List<AudioClip>();
    private IDictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    private AudioSource soundSource;

    private float currentSoundVolumn = 1f;

    void Start ()
    {
        OrganizeSfxDictionary();
        soundSource = GetComponent<AudioSource>();
    }	
	
	void Update () { }

    private void OrganizeSfxDictionary()
    {
        foreach(AudioClip sfx in sfxList)
        {
            sfxDictionary.Add(sfx.name, sfx);
        }
    }

    public void PlaySfx(string sfxName)
    {
        soundSource.PlayOneShot(sfxDictionary[sfxName], currentSoundVolumn);
    }

    public void ChangeSoundSourceVolume(float newVolumn)
    {
        soundSource.volume = newVolumn;
    }
}
