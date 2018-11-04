using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : Player {

    public enum Difficult
    {
        EASY = 1,
        MEDIUM = 2,
        HARD = 3,
        IMPOSSIBLE = 5
    }


    public Difficult level = Difficult.MEDIUM;
    private int number_of_actions_per_thought = 0;

    public float thought_time = 0;
    private float current_thought_time = 0;
    
	void Start ()
    {
        number_of_actions_per_thought = level.GetHashCode();
        team = TeamHelpers.Team.IA_TEAM;
    }

	void Update ()
    {
        if (current_thought_time > thought_time)
        {
            Think();
            current_thought_time = 0;
        } else {
            current_thought_time += Time.deltaTime;
        }
	}

    private void Think()
    {
        MachineTargetList list_of_targets = new MachineTargetList(AllMachines(), team);

        if (list_of_targets.BestTargets().Count > 0)
        {
            Debug.Log(list_of_targets.BestTargets().Count);
            int actions = number_of_actions_per_thought - (number_of_actions_per_thought - list_of_targets.BestTargets().Count);
            for(int i=0; i < actions; i++)
            {
                selected_machine = list_of_targets.BestTargets()[i].StrongestAllyMachineConnected();
                target_machine = list_of_targets.BestTargets()[i].Machine();
                TryAttackMachine();
            }
        }
    }

    private void TryAttackMachine()
    {
        if(selected_machine != null && target_machine != null)
        {
            AttackMachine();
        } else {
            DeselectAllMachines();
        }
    }

    private List<Machine> AllMachines()
    {
        return machine_controller.machines;
    }

    private List<Machine> IAMachines()
    {
        return AllMachines().FindAll(machine => machine.team == team);
    }

    private List<Machine> PlayerMachines()
    {
        return AllMachines().FindAll(machine => machine.team == TeamHelpers.Team.HUMAN_TEAM);
    }

    private List<Machine> NeutralMachines()
    {
        return AllMachines().FindAll(machine => machine.team == TeamHelpers.Team.HUMAN_TEAM);
    }
}
