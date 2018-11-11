using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour {

    public Text tip_text;
    public TipList tip_list = null;

    public string full_text = "";
    private string current_text = "";
    public float spelling_time = 0;
    public float blink_time = 0;
    private float current_time = 0;
    private bool end_of_text = false;

	void Start () { }
	
	void Update ()
    {
        if (Tick())
            if (end_of_text || full_text.Length == 0)
            {
                UpdateLastCharacter();
            } else {
                UpdateText();
                if (current_text.Length >= full_text.Length)
                    end_of_text = true;
            }
    }

    private bool Tick()
    {
        current_time += Time.deltaTime;
        if (current_time < CurrentDelayTime())
            return false;
        current_time = 0;
        return true;
    }

    private float CurrentDelayTime()
    {
        if (end_of_text)
            return blink_time;
        return spelling_time;
    }

    private void UpdateText()
    {
        current_text += full_text[current_text.Length];
        UpdateTipText();
    }

    private void UpdateLastCharacter()
    {
        if (current_text[current_text.Length - 1].Equals('|')) {
            current_text = current_text.Remove(current_text.Length -1);
        } else
        {
            current_text += "|";
        }
        UpdateTipText();
    }

    private void UpdateTipText()
    {
        tip_text.text = current_text;
    }

    public void NextTipOnList()
    {
        tip_list.NextTip();
    }
}
