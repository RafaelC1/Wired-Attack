using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextController : MonoBehaviour
{
    public GameController gameController = null;
    public GameObject tipListPreFab = null;
    public PopUpText popUpTextPreFab = null;
    public GameObject canvas = null;

    void Start()
    {
    }

    void Update()
    {
    }

    public void CreateTips(List<string> tips)
    {
        GameObject tipGo = Instantiate(tipListPreFab);
        tipGo.transform.SetParent(canvas.transform, false);
        TipList tipList = tipGo.GetComponent<TipList>();
        tipList.tips = tips;
        tipList.gameController = gameController;
    }

    public void CreatePopText(string text, Transform location)
    {
        PopUpText instance = Instantiate(popUpTextPreFab);

        //Vector2 screen_position = Camera.main.WorldToScreenPoint(location.position);
        Vector2 screen_position = new Vector2(location.position.x, location.position.y);

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screen_position;
        instance.SetText(text);
    }
}
