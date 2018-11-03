using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

    public TranslationController translation_controller = null;
    public SoundEffectController sound_controller = null;
    public MusicController music_controller = null;

    public Slider music_slider;
    public Slider sound_slider;

    public Text language_text_field;

    private string MUSIC_VOLUME_KEY = "music_volume";
    private string SOUND_VOLUME_KEY = "sound_volume";
    private string LANGUAGE_KEY = "user_language";

    void Start()
    {
        music_slider.onValueChanged.AddListener(delegate { VolumeChanged(); });
        sound_slider.onValueChanged.AddListener(delegate { VolumeChanged(); });

        DefineFirstGameSettings();

        LoadSavedSettings();
        UpdateSoundSourcesVolume();
    }

    void Update() { }

    public void NextLanguage()
    {
        translation_controller.NextLanguage();

        if (!translation_controller.CurrentLanguageExist())
        {
            translation_controller.SelectFirstLanguage();
        }
        translation_controller.UpdateAllTextFields();
        LanguageChanged();
    }

    public void BackLanguage()
    {
        translation_controller.BackLanguage();

        if (!translation_controller.CurrentLanguageExist())
        {
            translation_controller.SelectLastLanguage();
        }
        translation_controller.UpdateAllTextFields();
        LanguageChanged();
    }

    private void LanguageChanged()
    {
        language_text_field.text = translation_controller.CurrentLanguage();
    }

    private void VolumeChanged()
    {
        UpdateSoundSourcesVolume();
    }

    public void SaveCurrentSettings()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, music_slider.value);
        PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, sound_slider.value);
        PlayerPrefs.SetInt(LANGUAGE_KEY, translation_controller.current_language_id);
        PlayerPrefs.Save();
    }

    private void LoadSavedSettings()
    {
        LoadVolumeSettings();
        LoadLanguageeSettings();
        LanguageChanged();
    }

    private void LoadVolumeSettings()
    {
        music_slider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
        sound_slider.value = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY);
    }

    private void LoadLanguageeSettings()
    {
        translation_controller.current_language_id = PlayerPrefs.GetInt(LANGUAGE_KEY);
        translation_controller.UpdateAllTextFields();
    }

    private void UpdateSoundSourcesVolume()
    {
        music_controller.ChangeSoundSourceVolume(music_slider.value);
        sound_controller.ChangeSoundSourceVolume(sound_slider.value);
    }

    private void DefineFirstGameSettings()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME_KEY)) PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, 0.5f);
        if (!PlayerPrefs.HasKey(SOUND_VOLUME_KEY)) PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, 0.5f);
        if (!PlayerPrefs.HasKey(LANGUAGE_KEY)) PlayerPrefs.SetInt(LANGUAGE_KEY, 0);
    }
}
