using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MachineTarget {

    private Machine machine = null;
    public TeamHelpers.Team searcher_team = TeamHelpers.Team.NEUTRAL_TEAM;
    private int score = 0;
    
    public MachineTarget(Machine machine_target, TeamHelpers.Team searcher_team)
    {
        machine = machine_target;
        this.searcher_team = searcher_team;
    }

    public List<Machine> AlliesWithTheDoubleStoredBitsThanMine()
    {
        return AlliedConnected().Where(allied => allied.current_stored_bits > 2 * machine.current_stored_bits)
                                .ToList();
    }

    public List<Machine> AlliesWithMoreStoredBitsThanMy()
    {
        return AlliedConnected().Where(allied => allied.current_stored_bits > machine.current_stored_bits)
                                .ToList();
    }

    public Machine StrongestAllyMachineConnected()
    {
        int max = AlliedConnected().Max(machine => machine.current_stored_bits);
        if (max > 0)
        {
            return AlliedConnected().Find(machine => machine.current_stored_bits == max);
        } else
        {
            return null;
        }
    }

    private List<Machine> AllAnotherMachines()
    {
        return machine.ConnectedMachines().Where(machine => machine != this.machine).ToList();
    }

    public bool AnyAlliesConnected()
    {
        return AlliedConnected().Any();
    }

    public List<Machine> AlliedConnected()
    {
        return AllAnotherMachines().FindAll(connected_machine => connected_machine.team == searcher_team);
    }

    public bool AnyStrongEnemyConnected()
    {
        return EnemiesWithMoreStoredBitsThanMy().Any();
    }

    public bool AnyEnemieConnected()
    {
        return EnemiesConnected().Any();
    }

    public List<Machine> EnemiesWithDoubleStoredBitsThanMine()
    {
        return EnemiesWithMoreStoredBitsThanMy().FindAll(enemy => enemy.current_stored_bits > 2 * machine.current_stored_bits);
    }

    public List<Machine> EnemiesWithMoreStoredBitsThanMy()
    {
        return EnemiesConnected().Where(allied => allied.current_stored_bits > machine.current_stored_bits)
                                .ToList();
    }

    public List<Machine> EnemiesConnected()
    {
        return AllAnotherMachines().FindAll(connected_machine => connected_machine.team != searcher_team &&
                                                                 connected_machine.team != TeamHelpers.Team.NEUTRAL_TEAM);
    }

    public bool AnyNeutralConnected()
    {
        return NeutralsConnected().Any();
    }

    public List<Machine> NeutralsConnected()
    {
        return machine.ConnectedMachines()
                      .FindAll(connected_machine => connected_machine.team != TeamHelpers.Team.NEUTRAL_TEAM &&
                                                    connected_machine != machine);
    }

    public bool IsEnemy()
    {
        return machine.team != searcher_team;
    }

    public bool IsNeutral()
    {
        return machine.team == TeamHelpers.Team.NEUTRAL_TEAM;
    }

    public Machine Machine()
    {
        return machine;
    }

    public int Score()
    {
        return score;
    }

    public void AddScore(int score_to_add)
    {
        score += score_to_add;
    }
}
