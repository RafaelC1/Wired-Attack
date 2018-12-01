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
        Color teamColor = Color.white;
        switch(team)
        {
            case Team.HUMAN_TEAM:
            {
                teamColor = Color.green;
                break;
            }
            case Team.RED_TEAM:
            {
                teamColor = Color.red;
                break;
            }
            case Team.YELLOW_TEAM:
            {
                teamColor = Color.yellow;
                break;
            }
            case Team.NEUTRAL_TEAM:
            {
                teamColor = Color.gray;
                break;
            }
        }
        return teamColor; 
    }
    
}
