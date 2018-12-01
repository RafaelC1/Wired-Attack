using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour {

    public Text tipText;
    public TipList tipList = null;

    public string fullText = "";
    private string currentText = "";
    public float spellingTime = 0;
    public float blinkTime = 0;
    private float currentTime = 0;
    private bool endOfText = false;

	void Start () { }
	
	void Update ()
    {
        if (Tick())
            if (endOfText || fullText.Length == 0)
            {
                UpdateLastCharacter();
            } else {
                UpdateText();
                if (currentText.Length >= fullText.Length)
                    endOfText = true;
            }
    }

    private bool Tick()
    {
        currentTime += Time.deltaTime;
        if (currentTime < CurrentDelayTime())
            return false;
        currentTime = 0;
        return true;
    }

    private float CurrentDelayTime()
    {
        if (endOfText)
            return blinkTime;
        return spellingTime;
    }

    private void UpdateText()
    {
        currentText += fullText[currentText.Length];
        UpdateTipText();
    }

    private void UpdateLastCharacter()
    {
        if (currentText[currentText.Length - 1].Equals('|')) {
            currentText = currentText.Remove(currentText.Length -1);
        } else
        {
            currentText += "|";
        }
        UpdateTipText();
    }

    private void UpdateTipText()
    {
        tipText.text = currentText;
    }

    public void NextTipOnList()
    {
        tipList.NextTip();
    }
}
