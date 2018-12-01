using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

    public TranslationController translationController = null;
    public SoundEffectController soundController = null;
    public MusicController musicController = null;

    public Slider musicSlider;
    public Slider soundSlider;

    public Text languageTextField;

    private string MUSIC_VOLUME_KEY = "music_volume";
    private string SOUND_VOLUME_KEY = "sound_volume";
    private string LANGUAGE_KEY = "user_language";

    void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { VolumeChanged(); });
        soundSlider.onValueChanged.AddListener(delegate { VolumeChanged(); });

        DefineFirstGameSettings();

        LoadSavedSettings();
        UpdateSoundSourcesVolume();
    }

    void Update() { }

    public void NextLanguage()
    {
        translationController.NextLanguage();

        if (!translationController.CurrentLanguageExist())
        {
            translationController.SelectFirstLanguage();
        }
        translationController.UpdateAllTextFields();
        LanguageChanged();
    }

    public void BackLanguage()
    {
        translationController.BackLanguage();

        if (!translationController.CurrentLanguageExist())
        {
            translationController.SelectLastLanguage();
        }
        translationController.UpdateAllTextFields();
        LanguageChanged();
    }

    private void LanguageChanged()
    {
        languageTextField.text = translationController.CurrentLanguage();
    }

    private void VolumeChanged()
    {
        UpdateSoundSourcesVolume();
    }

    public void SaveCurrentSettings()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, soundSlider.value);
        PlayerPrefs.SetInt(LANGUAGE_KEY, translationController.currentLanguageId);
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
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
        soundSlider.value = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY);
    }

    private void LoadLanguageeSettings()
    {
        translationController.currentLanguageId = PlayerPrefs.GetInt(LANGUAGE_KEY);
        translationController.UpdateAllTextFields();
    }

    private void UpdateSoundSourcesVolume()
    {
        musicController.ChangeSoundSourceVolume(musicSlider.value);
        soundController.ChangeSoundSourceVolume(soundSlider.value);
    }

    private void DefineFirstGameSettings()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME_KEY)) PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, 0.5f);
        if (!PlayerPrefs.HasKey(SOUND_VOLUME_KEY)) PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, 0.5f);
        if (!PlayerPrefs.HasKey(LANGUAGE_KEY)) PlayerPrefs.SetInt(LANGUAGE_KEY, 0);
    }
}
