using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamHelpers  {

    public const int HUMAN_TEAM = 1;

    public static Color TeamColorOf(int team_id)
    {
        Color team_color = Color.gray;
        switch(team_id)
        {
            case HUMAN_TEAM:
            {
                team_color = Color.blue;
                break;
            }
            case 2:
            {
                team_color = Color.red;
                break;
            }
            case 3:
            {
                team_color = Color.yellow;
                break;
            }
        }
        return team_color; 
    }
    
}
