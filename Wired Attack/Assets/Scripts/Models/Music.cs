using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public string name = "";

    public AudioClip main_version = null;
    public AudioClip loop_version = null;

    private bool played = false;
    public bool play = false;
    public bool loop = false;

    void Start () { }
	
	void Update () { }

    public AudioClip Current()
    {
        if (played)
            return loop_version;
        return loop_version;
    }

    public bool AlreadyPlayed()
    {
        return played;
    }

    public void MarkAsPlayed()
    {
        played = true;
    }
}
