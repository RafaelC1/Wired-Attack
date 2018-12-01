using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour {

    public int totalTime = 0;
    public Text totalTimeText = null;
    private string currentMapName = null;
    private string TIME_KEY = "_time";

	void Start ()
    {
        LoadNestTime();
    }

	void Update () { }

    public void DefineTime(int time)
    {
        totalTime = time;
        UpdateTotalTimeText();
        SaveLevelScore();
    }

    public void DefineMapName(string map_name)
    {
        currentMapName = map_name;
    }

    private string TimeFormatted()
    {
        int minutes = totalTime / 60;
        int secounds = totalTime % 60;

        if (minutes > 0)
            return string.Format("{0}min {1}s", minutes, secounds);

        return string.Format("{0}s", secounds);
    }

    public void UpdateTotalTimeText()
    {
        totalTimeText.text = TimeFormatted();
    }

    public void SaveLevelScore()
    {
        if (currentMapName != null &&
            (totalTime < PlayerPrefs.GetInt(TimeKey()) || !PlayerPrefs.HasKey(TimeKey())))
            PlayerPrefs.SetInt(TimeKey(), totalTime);
    }

    private string TimeKey()
    {
        return currentMapName + TIME_KEY;
    }

    public void LoadNestTime()
    {
        totalTime = 0;
        if (PlayerPrefs.HasKey(TimeKey()))
            totalTime = PlayerPrefs.GetInt(TimeKey(), totalTime);

        UpdateTotalTimeText();
    }

    private void OnEnable()
    {
        UpdateTotalTimeText();
    }
}
