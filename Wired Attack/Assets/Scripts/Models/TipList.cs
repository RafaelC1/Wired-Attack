using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipList : MonoBehaviour {

    public GameController game_controller = null;
    public GameObject tip_pre_fab = null;
    private GameObject current_tip_go = null;

    public List<string> tips = new List<string>();
    private int current_tip = -1;

	void Start ()
    {
        NextTip();

    }
	
	void Update () { }

    public void NextTip()
    {
        if (MaxTips() == current_tip)
        {
            game_controller.Resume();
            Destroy(this.gameObject);
            return;
        }
        current_tip++;
        CreateTip(CurrentTipText());
    }

    private string CurrentTipText()
    {
        return tips[current_tip];
    }

    private void CreateTip(string tip_text)
    {
        if (current_tip_go != null) Destroy(current_tip_go);

        current_tip_go = Instantiate(tip_pre_fab);
        Tip tip = current_tip_go.GetComponent<Tip>();

        current_tip_go.transform.SetParent(this.transform, false);
        tip.full_text = tip_text;
        tip.tip_list = this;
    }

    private int MaxTips()
    {
        return tips.Count - 1;
    }
}
