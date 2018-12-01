using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipList : MonoBehaviour {

    public GameController gameController = null;
    public GameObject tipPreFab = null;
    private GameObject currentTipGo = null;

    public List<string> tips = new List<string>();
    private int currentTip = -1;

	void Start ()
    {
        NextTip();

    }
	
	void Update () { }

    public void NextTip()
    {
        if (MaxTips() == currentTip)
        {
            gameController.Resume();
            Destroy(this.gameObject);
            return;
        }
        currentTip++;
        CreateTip(CurrentTipText());
    }

    private string CurrentTipText()
    {
        return tips[currentTip];
    }

    private void CreateTip(string tipText)
    {
        if (currentTipGo != null) Destroy(currentTipGo);

        currentTipGo = Instantiate(tipPreFab);
        Tip tip = currentTipGo.GetComponent<Tip>();

        currentTipGo.transform.SetParent(this.transform, false);
        tip.fullText = tipText;
        tip.tipList = this;
    }

    private int MaxTips()
    {
        return tips.Count - 1;
    }
}
