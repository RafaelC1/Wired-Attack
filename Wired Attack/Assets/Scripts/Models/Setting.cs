using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

    public TranslationController translation_controller = null;

    public Slider music_slider;
    public Slider sound_slider;

    public Text language_text_field;

    public float music_volumn = 1f;
    public float sound_volumn = 1f;

    void Start()
    {
        //LoadSavedSettings();
        music_slider.onValueChanged.AddListener(delegate { MusicVolumnChanged(); });
        sound_slider.onValueChanged.AddListener(delegate { SoundVolumnChanged(); });
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
        UpdateLanguateTextField();
    }

    public void BackLanguage()
    {
        translation_controller.BackLanguage();

        if (!translation_controller.CurrentLanguageExist())
        {
            translation_controller.SelectLastLanguage();
        }
        translation_controller.UpdateAllTextFields();
        UpdateLanguateTextField();
    }

    private void UpdateLanguateTextField()
    {
        language_text_field.text = translation_controller.CurrentLanguage();
    }

    private void MusicVolumnChanged()
    {
        music_volumn = music_slider.value;
        SaveCurrentSettings();
    }

    private void SoundVolumnChanged()
    {
        sound_volumn = sound_slider.value;
        SaveCurrentSettings();
    }

    private void SaveCurrentSettings()
    {

    }

    private void LoadSavedSettings()
    {

        translation_controller.UpdateAllTextFields();
        UpdateLanguateTextField();
    }
}
