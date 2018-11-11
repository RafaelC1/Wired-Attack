using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextController : MonoBehaviour
{
    public GameController game_controller = null;
    public GameObject tip_list_pre_fab = null;
    public PopUpText pop_up_text_pre_fab = null;
    public GameObject canvas = null;

    void Start()
    {
    }

    void Update()
    {
    }

    public void CreateTips(List<string> tips)
    {
        GameObject tip_go = Instantiate(tip_list_pre_fab);
        tip_go.transform.SetParent(canvas.transform, false);
        TipList tip_list = tip_go.GetComponent<TipList>();
        tip_list.tips = tips;
        tip_list.game_controller = game_controller;
    }

    public void CreatePopText(string text, Transform location)
    {
        PopUpText instance = Instantiate(pop_up_text_pre_fab);

        //Vector2 screen_position = Camera.main.WorldToScreenPoint(location.position);
        Vector2 screen_position = new Vector2(location.position.x, location.position.y);

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screen_position;
        instance.SetText(text);
    }
}
