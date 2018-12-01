using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MachineTarget {

    private Machine machine = null;
    public TeamHelpers.Team searcherTeam = TeamHelpers.Team.NEUTRAL_TEAM;
    private int score = 0;
    
    public MachineTarget(Machine machineTarget, TeamHelpers.Team searcherTeam)
    {
        machine = machineTarget;
        this.searcherTeam = searcherTeam;
    }

    public bool AnyEnemyMessageOnWay()
    {
        return Machine().AllMyMessagesOnMyWay().Find(msg => msg.fromTeam != searcherTeam);
    }

    public bool AnyAlliedMessageOnWay()
    {
        return Machine().AllMyMessagesOnMyWay().Find(msg => msg.fromTeam == searcherTeam);
    }

    public List<Machine> AllMachinesConnected()
    {
        return machine.ConnectedMachines();
    }

    public bool AnyAllyWithDoubleBitsConnected()
    {
        return StrongestAllyConnected().currentStoredBits > 2 * Machine().currentStoredBits;
    }

    public bool AnyAllyWithMoreBitsConnected()
    {
        return StrongestAllyConnected().currentStoredBits > Machine().currentStoredBits;
    }

    public bool AnyAllyConnected()
    {
        return AllAlliesConnected().Any();
    }

    public Machine StrongestAllyConnected()
    {
        int max = AllAlliesConnected().Max(machine => machine.currentStoredBits);
        return AllAlliesConnected().Find(machine => machine.currentStoredBits == max);
    }

    public bool AnyAllyConnectedWithBits()
    {
        return AllAlliesConnected().Find(machine => !machine.IsStorageEmpty());
    }

    public List<Machine> AllAlliesConnected()
    {
        return AllMachinesConnected().FindAll(machine => machine.team == searcherTeam);
    }

    public bool AnyEnemyConnected()
    {
        return AllEnemiesConnected().Any();
    }

    public List<Machine> AllEnemiesConnected()
    {
        return AllMachinesConnected().FindAll(connected_machine => connected_machine.team != searcherTeam &&
                                                                   connected_machine.team != TeamHelpers.Team.NEUTRAL_TEAM);
    }

    public bool AnyNeutralConnected()
    {
        return AllNeutralsConnected().Any();
    }

    public List<Machine> AllNeutralsConnected()
    {
        return machine.ConnectedMachines()
                      .FindAll(connectedMachine => connectedMachine.IsNeutral() &&
                                                   connectedMachine != machine);
    }

    public bool MachineIsEnemy()
    {
        return !MachineIsNeutral() && machine.team != searcherTeam;
    }

    public bool MachineIsNeutral()
    {
        return machine.IsNeutral();
    }

    public bool MachineIsAllied()
    {
        return machine.team == searcherTeam;
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
        return Machine().produceBit;
    }

    public Machine Machine()
    {
        return machine;
    }

    public int Score()
    {
        return score;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
}
