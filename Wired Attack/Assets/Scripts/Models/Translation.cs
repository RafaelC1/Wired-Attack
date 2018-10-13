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
        translation_controller.AddTranslationManager(this);
        text_field = GetComponent<Text>();
	}
	
	void Update () { }

    public void ChangeText(string new_text)
    {
        text_field.text = new_text;
    }
}
