using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IADifficult : MonoBehaviour {

    public enum Difficult
    {
        EASY,
        MEDIUM,
        HARD,
        IMPOSSIBLE
    }

    public string name = "";
    public Difficult difficult = Difficult.EASY;
    public float thought_time = 1f;
    public int number_of_actions_per_thought = 1;

}
