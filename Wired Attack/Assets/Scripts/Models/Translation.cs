using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translation : MonoBehaviour {

    public TranslationController translationController = null;
    public string textKey = null;
    private Text textField = null;

	void Start ()
    {
        if (translationController == null)
            translationController = GameObject.Find("TranslationController")
                                              .GetComponent<TranslationController>(); // soo wrong but whatever
        translationController.AddTranslationManager(this);
        textField = GetComponent<Text>();
        UpdateMyText();
    }
	
	void Update () { }

    public void UpdateMyText()
    {
        textField.text = translationController.TranslationByKey(textKey);
    }

    public void ChangeText(string new_text)
    {
        textField.text = new_text;
    }
}
