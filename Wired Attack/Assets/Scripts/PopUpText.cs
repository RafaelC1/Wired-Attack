using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour {

    public Animator animator = null;
    public Text text = null;


	void Start () {
        AnimatorClipInfo[] clip_info = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clip_info[0].clip.length);
	}
	
	void Update () {
		
	}

    public void SetText(string new_text)
    {
        text.text = new_text;
    }
}
