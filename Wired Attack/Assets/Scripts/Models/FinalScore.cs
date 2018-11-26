using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour {

    public int total_time = 0;
    public Text total_time_text = null;
    private string current_map_name = null;
    private string TIME_KEY = "_time";

	void Start ()
    {
        LoadNestTime();
    }

	void Update () { }

    public void DefineTime(int time)
    {
        total_time = time;
        UpdateTotalTimeText();
        SaveLevelScore();
    }

    public void DefineMapName(string map_name)
    {
        current_map_name = map_name;
    }

    private string TimeFormatted()
    {
        int minutes = total_time / 60;
        int secounds = total_time % 60;

        if (minutes > 0)
            return string.Format("{0}min {1}s", minutes, secounds);

        return string.Format("{0}s", secounds);
    }

    public void UpdateTotalTimeText()
    {
        total_time_text.text = TimeFormatted();
    }

    public void SaveLevelScore()
    {
        if (current_map_name != null &&
            (total_time < PlayerPrefs.GetInt(TimeKey()) || !PlayerPrefs.HasKey(TimeKey())))
            PlayerPrefs.SetInt(TimeKey(), total_time);
    }

    private string TimeKey()
    {
        return current_map_name + TIME_KEY;
    }

    public void LoadNestTime()
    {
        total_time = 0;
        if (PlayerPrefs.HasKey(TimeKey()))
            total_time = PlayerPrefs.GetInt(TimeKey(), total_time);

        UpdateTotalTimeText();
    }

    private void OnEnable()
    {
        UpdateTotalTimeText();
    }
}
