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

    public List<Machine> AllMachinesConnected()
    {
        return machine.ConnectedMachines();
    }

    public bool AnyAllyWithDoubleBitsConnected()
    {
        return StrongestAllyConnected().current_stored_bits > 2*Machine().current_stored_bits;
    }

    public bool AnyAllyWithMoreBitsConnected()
    {
        return StrongestAllyConnected().current_stored_bits > Machine().current_stored_bits;
    }

    public bool AnyAllyConnected()
    {
        return AllAlliesConnected().Any();
    }

    public Machine StrongestAllyConnected()
    {
        int max = AllAlliesConnected().Max(machine => machine.current_stored_bits);
        return AllAlliesConnected().Find(machine => machine.current_stored_bits == max);
    }

    public List<Machine> AllAlliesConnected()
    {
        return AllMachinesConnected().FindAll(machine => machine.team == searcher_team);
    }

    public bool AnyEnemyConnected()
    {
        return AllEnemiesConnected().Any();
    }

    public List<Machine> AllEnemiesConnected()
    {
        return AllMachinesConnected().FindAll(connected_machine => connected_machine.team != searcher_team &&
                                                                 connected_machine.team != TeamHelpers.Team.NEUTRAL_TEAM);
    }

    public bool AnyNeutralConnected()
    {
        return AllNeutralsConnected().Any();
    }

    public List<Machine> AllNeutralsConnected()
    {
        return machine.ConnectedMachines()
                      .FindAll(connected_machine => connected_machine.IsNeutral() &&
                                                    connected_machine != machine);
    }

    public bool MachineIsEnemy()
    {
        return !MachineIsNeutral() && machine.team != searcher_team;
    }

    public bool MachineIsNeutral()
    {
        return machine.IsNeutral();
    }

    public bool MachineIsAllied()
    {
        return machine.team == searcher_team;
    }

    public bool IsMachineStorageFull()
    {
        return Machine().IsStorageFull();
    }

    public bool IsMachineStorageEmpty()
    {
        return Machine().IsStorageEmpty();
    }

    public bool IsMachineStorageHalfFull()
    {
        return !IsMachineStorageFull() && !IsMachineStorageEmpty();
    }

    public bool MachineCanProduceBits()
    {
        return Machine().produce_bit;
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
