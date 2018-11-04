using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MachineTargetList {

    private List<MachineTarget> machine_targets = new List<MachineTarget>();
    private TeamHelpers.Team search_team = TeamHelpers.Team.NEUTRAL_TEAM;
	
    public MachineTargetList(List<Machine> machines, TeamHelpers.Team team)
    {
        search_team = team;
        foreach(Machine machine in machines)
        {
            machine_targets.Add(new MachineTarget(machine, team));
        }

        DefineScore();
    }

    private void DefineScore()
    {
        foreach (MachineTarget target in machine_targets)
        {
            if (target.AnyAlliesConnected())
            {
                // TODO FIX THE SCORE
                if (target.IsNeutral()) ScoreForNeutral(target);
                if (target.IsEnemy()) ScoreForEnemy(target);
                if (!target.IsEnemy() && !target.IsNeutral()) ScoreForAlly(target);
            }
            continue;
            if(target.AnyAlliesConnected()) // first of all, a machine not conected dont worth nothing
            {
                Machine machine = target.Machine();

                if (target.IsNeutral()) // priorize neutral machines because they are weak
                {
                    target.AddScore(3);
                    if(target.AnyEnemieConnected())
                        target.AddScore(1);
                }
                else if (target.IsEnemy()) // if there is only enemies left...
                {
                    target.AddScore(2);
                    if (target.AlliesWithTheDoubleStoredBitsThanMine().Any()) // if an enemy is conected to an strong ally, use to attack
                        target.AddScore(1);
                } else { //if is an ally...
                    target.AddScore(1);
                    if (target.AnyEnemieConnected())
                    {
                        int extra = 1;
                        if (target.EnemiesWithMoreStoredBitsThanMy().Any())
                            extra = 2;
                        if (target.EnemiesWithDoubleStoredBitsThanMine().Any())
                            extra = 3;

                        if (machine.IsStorageFull())
                        {
                            target.AddScore(-1);
                        } else if (!machine.IsStorageFull() && !machine.IsStorageEmpty())
                        {
                            target.AddScore(1*extra);
                        } else if(machine.IsStorageEmpty())
                        {
                            target.AddScore(2*extra);
                        }
                    } else if (target.AnyNeutralConnected())
                    {
                        target.AddScore(2);
                    } else {
                        target.AddScore(-1);
                    }
                }
            } else {
                target.AddScore(-1);
            }
        }
    }

    private void ScoreForNeutral(MachineTarget target)
    {
        int score = 4;

        if (target.AnyAlliesConnected())
            score += 1;
        if (target.AnyEnemieConnected())
            score += 2;
        if (target.AnyStrongEnemyConnected())
            score += 1;

        target.AddScore(score);
    }

    private void ScoreForEnemy(MachineTarget target)
    {
        int score = 2;

        if (target.AlliesWithMoreStoredBitsThanMy().Any())
            score += 1;
        if (target.AlliesWithTheDoubleStoredBitsThanMine().Any())
            score += 1;        

        target.AddScore(score);
    }

    private void ScoreForAlly(MachineTarget target)
    {
        int score = 1;
        Machine machine = target.Machine();

        if (target.AnyStrongEnemyConnected())
        {
            if (target.StrongestAllyMachineConnected())
            {
                score += 2;
            } else {
                score += 1;
            }
        }

        if (machine.IsStorageEmpty())
        {
            score += 1;
            if (target.AnyEnemieConnected())
                score += 1;
        }

        if (!machine.IsStorageEmpty() && !machine.IsStorageFull())
        {
            if (target.AnyStrongEnemyConnected())
                score += 1;
        }

        if (target.AnyNeutralConnected())
            score += 3;

        target.AddScore(score);
    }

    public List<MachineTarget> BestTargets()
    {
        int max_score = machine_targets.Max(target => target.Score());
        return machine_targets.Where(target => target.AlliedConnected().Count > 0)
                              .Where(target => target.Score() > 0)
                              .OrderByDescending(target => target.Score())
                              .ToList();
                              
    }
}
