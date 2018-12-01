using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : Player {

    public IADifficult.Difficult level = IADifficult.Difficult.MEDIUM;

    public List<GameObject> difficultSettings = new List<GameObject>();
    
    private float currentThoughtTime = 0;
    
	void Start () { }

	void Update ()
    {
        if (gameController.IsPaused()) return;
        if (currentThoughtTime > CurrentDifficult().thoughtTime)
        {
            Think();
            currentThoughtTime = 0;
        } else {
            currentThoughtTime += Time.deltaTime;
        }
	}

    private void Think()
    {
        MachineTargetList list_of_targets = new MachineTargetList(AllMachines(), team);

        if (list_of_targets.BestTargets().Count > 0)
        {
            for(int i=0; i < CurrentDifficult().numberOfActionsPerThought; i++)
            {
                if (i >= list_of_targets.BestTargets().Count)
                    break;
                selectedMachine = list_of_targets.BestTargets()[i].StrongestAllyConnected();
                targetMachine = list_of_targets.BestTargets()[i].Machine();
                TryAttackMachine();
            }
        }
    }

    private void TryAttackMachine()
    {
        if(selectedMachine != null &&
           targetMachine != null &&
           !selectedMachine.IsStorageEmpty())
        {
            AttackMachine();
        } else {
            DeselectAllMachines();
        }
    }

    private List<Machine> AllMachines()
    {
        return machineController.machines;
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
        return difficultSettings.Find(difficult => difficult.GetComponent<IADifficult>().difficult == level)
                                .GetComponent<IADifficult>();
    }
}
