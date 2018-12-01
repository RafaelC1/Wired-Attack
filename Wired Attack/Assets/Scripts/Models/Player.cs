using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public MachineController machineController = null;
    public GameController gameController = null;

    protected string playerName = "Rafael";
    public TeamHelpers.Team team = TeamHelpers.Team.HUMAN_TEAM;

    public Machine selectedMachine = null;
    public Machine targetMachine = null;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (gameController.IsPaused()) return;
        DetectClickOnMachines();
    }

    private void DetectClickOnMachines()
    {
        if (!TouchHelpers.IsTouchingOrClickingOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit && hit.transform.tag == "machine")
            {
                Machine machine_hit = hit.transform.GetComponent<Machine>(); ;
                if (machine_hit.machineController.IsThereMoreThanOneTeamAlive())
                    if (selectedMachine == null)
                    {
                        if (machine_hit.team == TeamHelpers.Team.HUMAN_TEAM)
                        {
                            SelectMachine(machine_hit);
                        }
                    } else {
                        if (selectedMachine == machine_hit)
                        {
                            DeselectAllMachines();
                        } else {
                            SelectMachine(machine_hit);
                        }                    
                    }
            }

            if (selectedMachine != null && targetMachine != null)
            {
                AttackMachine();
            }
        }
    }

    protected void AttackMachine()
    {
        machineController.TryTransferBitsBetweenMachines(targetMachine, selectedMachine);
        DeselectAllMachines();
    }

    protected void SelectMachine(Machine machine)
    {
        if (selectedMachine == null)
        {
            selectedMachine = machine;
            selectedMachine.ActiveBackGround(true);
        } else {
            targetMachine = machine;
            targetMachine.ActiveBackGround(true);
        }
    }

    protected void DeselectAllMachines()
    {
        if (selectedMachine != null)
        {
            selectedMachine.ActiveBackGround(false);
            selectedMachine = null;
        }

        if (targetMachine != null)
        {
            targetMachine.ActiveBackGround(false);
            targetMachine = null;
        }
    }
}
