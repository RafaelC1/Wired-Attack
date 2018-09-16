using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TeamHelpers  {

    public const int NEUTRAL_TEAM = 0;
    public const int HUMAN_TEAM = 1;
    public const int IA_TEAM = 2;

    public static Color TeamColorOf(int team_id)
    {
        Color team_color = Color.white;
        switch(team_id)
        {
            case HUMAN_TEAM:
            {
                team_color = Color.blue;
                break;
            }
            case IA_TEAM:
            {
                team_color = Color.red;
                break;
            }
            case NEUTRAL_TEAM:
            {
                team_color = Color.gray;
                break;
            }
        }
        return team_color; 
    }
    
}
