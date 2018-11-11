using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : Player {

    public IADifficult.Difficult level = IADifficult.Difficult.MEDIUM;

    public List<GameObject> difficult_settings = new List<GameObject>();
    
    private float current_thought_time = 0;
    
	void Start () { }

	void Update ()
    {
        if (game_controller.isPaused()) return;
        if (current_thought_time > CurrentDifficult().thought_time)
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
            for(int i=0; i < CurrentDifficult().number_of_actions_per_thought; i++)
            {
                if (i >= list_of_targets.BestTargets().Count)
                    break;
                selected_machine = list_of_targets.BestTargets()[i].StrongestAllyConnected();
                target_machine = list_of_targets.BestTargets()[i].Machine();
                TryAttackMachine();
            }
        }
    }

    private void TryAttackMachine()
    {
        if(selected_machine != null &&
           target_machine != null &&
           !selected_machine.IsStorageEmpty())
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
    
    private IADifficult CurrentDifficult()
    {
        return difficult_settings.Find(difficult => difficult.GetComponent<IADifficult>().difficult == level)
                                 .GetComponent<IADifficult>();
    }
}
