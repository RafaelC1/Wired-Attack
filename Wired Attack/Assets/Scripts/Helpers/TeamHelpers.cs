using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TeamHelpers  {

    public enum Team
    {
        NEUTRAL_TEAM,
        HUMAN_TEAM,
        RED_TEAM,
        YELLOW_TEAM
    }

    public static Color TeamColorOf(Team team)
    {
        Color team_color = Color.white;
        switch(team)
        {
            case Team.HUMAN_TEAM:
            {
                team_color = Color.blue;
                break;
            }
            case Team.RED_TEAM:
            {
                team_color = Color.red;
                break;
            }
            case Team.YELLOW_TEAM:
            {
                team_color = Color.yellow;
                break;
            }
            case Team.NEUTRAL_TEAM:
            {
                team_color = Color.gray;
                break;
            }
        }
        return team_color; 
    }
    
}
