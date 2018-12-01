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
    
    public Difficult difficult = Difficult.EASY;
    public float thoughtTime = 1f;
    public int numberOfActionsPerThought = 1;

}
