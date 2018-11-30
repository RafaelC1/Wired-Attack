using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationController : MonoBehaviour {

    private const char SYMBOL_TO_DIVID_KEY_AND_VALUE = ':';

    public List<TextAsset> translation_files = new List<TextAsset>();
    private IDictionary<string, IDictionary<string, string>> translations = new Dictionary<string, IDictionary<string, string>>();

    public List<string> available_languages = new List<string>();
    public int current_language_id = 0;

    private List<Translation> text_field_translations = new List<Translation>();

    void Start ()
    {
        OrganizeTranslations();
    }

	void Update () { }

    private void OrganizeTranslations()
    {
        foreach(TextAsset translation_file in translation_files)
        {
            available_languages.Add(translation_file.name);

            translations.Add(translation_file.name, TranslationFileContent(translation_file));
        }
    }

    private IDictionary<string, string> TranslationFileContent(TextAsset file)
    {
        IDictionary<string, string> key_value_of_translations = new Dictionary<string, string>();

        List<string> all_lines = new List<string>(file.text.Split("\n"[0]));

        foreach(string line in all_lines)
        {
            string key = line.Split(SYMBOL_TO_DIVID_KEY_AND_VALUE)[0];
            string value = line.Split(SYMBOL_TO_DIVID_KEY_AND_VALUE)[1];

            key_value_of_translations.Add(key, value);
        }

        return key_value_of_translations;
    }

    public void UpdateAllTextFields()
    {
        foreach(Translation translation in text_field_translations)
        {
            translation.ChangeText(TranslationByKey(translation.text_key));
        }
    }

    public string CurrentLanguage()
    {
        try {
            return available_languages[current_language_id];
        } catch (System.Exception) {
            return "";
        }
    }

    public string TranslationByKey(string key)
    {
        string translation = null;

        try
        {
            translation = translations[CurrentLanguage()][key];
        } catch(KeyNotFoundException e) {
            //Debug.Log("key: " + key + " not found");
            translation = key;
        }
        return translation;
    }

    public void AddTranslationManager(Translation new_translation)
    {
        text_field_translations.Add(new_translation);
    }

    public int CountOfAvailableLanguages()
    {
        return available_languages.Count - 1;
    }

    public void NextLanguage()
    {
        current_language_id++;
    }

    public void BackLanguage()
    {
        current_language_id--;
    }

    public void SelectLastLanguage()
    {
        current_language_id = CountOfAvailableLanguages();
    }

    public void SelectFirstLanguage()
    {
        current_language_id = 0;
    }

    public bool CurrentLanguageExist()
    {
        return current_language_id <= CountOfAvailableLanguages() &&
               current_language_id >= 0;
    }
}
