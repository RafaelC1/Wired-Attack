using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public MachineController machine_controller = null;

    public List<Machine> list_of_allied_machines = new List<Machine>();
    public List<Machine> list_of_enemy_machines = new List<Machine>();
    public List<Machine> list_of_neutral_machines = new List<Machine>();

    void Start ()
    {
        OrganizeAllMachinesInTheList();
    }	
	
	void Update ()
    {
        ProcessActions();
    }

    private void ProcessActions()
    {
        foreach(Machine allied_machine in list_of_allied_machines)
        {

        }
    }

    private bool CheckConnectionBetweenTwoMachines(Machine first_machine, Machine secound_machine)
    {
        return first_machine.IsConnectedTo(secound_machine);
    }

    #region methods to organize list of machines

    public void OrganizeAllMachinesInTheList()
    {
        OrganizeListOfMachines(machine_controller.machines);
    }

    public void OrganizeSingleMachineInTheList(Machine machine_to_organize)
    {
        OrganizeListOfMachines(new List<Machine>() { machine_to_organize });
    }

    private void OrganizeListOfMachines(List<Machine> list_of_machine_to_organize)
    {
        foreach (Machine machine in list_of_machine_to_organize)
        {
            if (machine.team_id == TeamHelpers.HUMAN_TEAM)
            {
                AddMachineToTheEnemyList(machine);
            } else if (machine.team_id == TeamHelpers.IA_TEAM) {
                AddMachineToTheAlliedList(machine);
            } else {
                AddMachineToTheEnemyList(machine);
            }
        }
    }

    private void AddMachineToTheAlliedList(Machine machine)
    {
        if (list_of_enemy_machines.Contains(machine)) { list_of_enemy_machines.Remove(machine); }
        if (list_of_neutral_machines.Contains(machine)) { list_of_neutral_machines.Remove(machine); }

        if (!list_of_allied_machines.Contains(machine)) { list_of_allied_machines.Add(machine); }
    }

    private void AddMachineToTheEnemyList(Machine machine)
    {
        if (list_of_allied_machines.Contains(machine)) { list_of_allied_machines.Remove(machine); }
        if (list_of_neutral_machines.Contains(machine)) { list_of_neutral_machines.Remove(machine); }

        if (!list_of_enemy_machines.Contains(machine)) { list_of_enemy_machines.Add(machine); }
    }

    private void AddMachineToTheNeutralList(Machine machine)
    {
        if (list_of_allied_machines.Contains(machine)) { list_of_allied_machines.Remove(machine); }
        if (list_of_enemy_machines.Contains(machine)) { list_of_enemy_machines.Remove(machine); }

        if (!list_of_neutral_machines.Contains(machine)) { list_of_neutral_machines.Add(machine); }
    }

    #endregion
}