using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour {

    public enum Languages { eng, pt, esp };
    public Languages current_language = Languages.eng;

    public float music_volumn = 1f;
    public float sound_volumn = 1f;

    public bool sound_on = true;
    public bool music_on = true;

    void Start() { }

    void Update() { }

    public void MuteMusic(bool mute)
    {
        music_on = mute;
    }

    public void MuteSound(bool mute)
    {
        sound_on = mute;
    }

    private void SaveCurrentSettings()
    {

    }

    private void LoadSavedSettings()
    {

    }
}
