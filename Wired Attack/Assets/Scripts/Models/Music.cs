using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public AudioClip intro = null;
    public AudioClip main = null;

    public bool play = true;
    public bool introPlayed = false;
    public bool mainPlayed = false;

    void Start () { }
	
	void Update () { }

    public AudioClip CurrentPart()
    {
        if (!introPlayed)
            return intro;
        return main;
    }

    public void NextPart()
    {
        if (!introPlayed)
        {
            introPlayed = true;
            return;
        }

        mainPlayed = true;
    }

    public void Reset()
    {
        mainPlayed = introPlayed = false;        
    }
}
