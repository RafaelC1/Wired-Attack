using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translation : MonoBehaviour {

    public TranslationController translation_controller = null;
    public string text_key = null;
    private Text text_field = null;

	void Start ()
    {
        if (translation_controller == null)
            translation_controller = GameObject.Find("TranslationController")
                                               .GetComponent<TranslationController>(); // soo wrong but for now its ok
        translation_controller.AddTranslationManager(this);
        text_field = GetComponent<Text>();
        UpdateMyText();
    }
	
	void Update () { }

    public void UpdateMyText()
    {
        text_field.text = translation_controller.TranslationByKey(text_key);
    }

    public void ChangeText(string new_text)
    {
        text_field.text = new_text;
    }
}
