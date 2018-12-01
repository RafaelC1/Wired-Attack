using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationController : MonoBehaviour {

    private const char SYMBOL_TO_DIVID_KEY_AND_VALUE = ':';

    public List<TextAsset> translationFiles = new List<TextAsset>();
    private IDictionary<string, IDictionary<string, string>> translations = new Dictionary<string, IDictionary<string, string>>();

    public List<string> availableLanguages = new List<string>();
    public int currentLanguageId = 0;

    private List<Translation> textFieldTranslations = new List<Translation>();

    void Start ()
    {
        OrganizeTranslations();
    }

	void Update () { }

    private void OrganizeTranslations()
    {
        foreach(TextAsset translationFiles in translationFiles)
        {
            availableLanguages.Add(translationFiles.name);

            translations.Add(translationFiles.name, TranslationFileContent(translationFiles));
        }
    }

    private IDictionary<string, string> TranslationFileContent(TextAsset file)
    {
        IDictionary<string, string> keyValueOfTranslation = new Dictionary<string, string>();

        List<string> all_lines = new List<string>(file.text.Split("\n"[0]));

        foreach(string line in all_lines)
        {
            string key = line.Split(SYMBOL_TO_DIVID_KEY_AND_VALUE)[0];
            string value = line.Split(SYMBOL_TO_DIVID_KEY_AND_VALUE)[1];

            keyValueOfTranslation.Add(key, value);
        }

        return keyValueOfTranslation;
    }

    public void UpdateAllTextFields()
    {
        foreach(Translation translation in textFieldTranslations)
        {
            translation.ChangeText(TranslationByKey(translation.textKey));
        }
    }

    public string CurrentLanguage()
    {
        try {
            return availableLanguages[currentLanguageId];
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

    public void AddTranslationManager(Translation newTranslation)
    {
        textFieldTranslations.Add(newTranslation);
    }

    public int CountOfAvailableLanguages()
    {
        return availableLanguages.Count - 1;
    }

    public void NextLanguage()
    {
        currentLanguageId++;
    }

    public void BackLanguage()
    {
        currentLanguageId--;
    }

    public void SelectLastLanguage()
    {
        currentLanguageId = CountOfAvailableLanguages();
    }

    public void SelectFirstLanguage()
    {
        currentLanguageId = 0;
    }

    public bool CurrentLanguageExist()
    {
        return currentLanguageId <= CountOfAvailableLanguages() &&
               currentLanguageId >= 0;
    }
}
