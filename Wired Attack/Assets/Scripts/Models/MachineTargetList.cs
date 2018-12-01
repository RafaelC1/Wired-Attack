using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MachineTargetList {

    private List<MachineTarget> machineTargets = new List<MachineTarget>();
    private TeamHelpers.Team searchTeam = TeamHelpers.Team.NEUTRAL_TEAM;
	
    public MachineTargetList(List<Machine> machines, TeamHelpers.Team team)
    {
        searchTeam = team;
        foreach(Machine machine in machines)
        {
            machineTargets.Add(new MachineTarget(machine, team));
        }

        DefineScore();
    }

    private void DefineScore()
    {
        foreach (MachineTarget target in machineTargets)
        {
            if (target.AnyAllyConnected())
            {
                if (target.MachineIsNeutral())
                {
                    if (target.AnyAlliedMessageOnWay())
                    {
                        target.AddScore(1);
                    } else {
                        target.AddScore(3);
                    }
                    if (target.AnyEnemyConnected())
                        if (target.AnyEnemyMessageOnWay())
                        {
                            target.AddScore(3);
                        } else {
                            target.AddScore(2);
                        }
                } else if (target.MachineIsEnemy())
                {
                    if (target.AnyAllyWithDoubleBitsConnected())
                    {
                        target.AddScore(3);
                    } else if (target.AnyAllyWithMoreBitsConnected()) {
                        target.AddScore(2);
                    } else {
                        target.AddScore(1);
                    }
                } else if (target.MachineIsAllied())
                {
                    if (target.IsMachineStorageEmpty())
                        if (target.AnyEnemyConnected())
                        {
                            target.AddScore(2);
                        } else {
                            target.AddScore(0);
                        }
                    else if (target.IsMachineStorageHalfFull())
                    {
                        if (target.AnyEnemyConnected())
                        {
                            target.AddScore(3);
                        } else if (target.AnyNeutralConnected())
                        {
                            target.AddScore(1);
                        } else if (!target.MachineCanProduceBits())
                        {
                            target.AddScore(1);
                        }
                    } 
                }
            }
        }
    }

    public List<MachineTarget> BestTargets()
    {
        int max_score = machineTargets.Max(target => target.Score());
        return machineTargets.Where(target => target.AnyAllyConnected())
                             .Where(target => target.Score() > 0)
                             .Where(target => target.AnyAllyConnectedWithBits())
                             .OrderByDescending(target => target.Score())
                             .ToList();
                              
    }
}
