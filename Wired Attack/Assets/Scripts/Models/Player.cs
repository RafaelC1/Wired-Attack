using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public MachineController machine_controller = null;

    public string player_name = "Rafael";
    public int team_id = TeamHelpers.HUMAN_TEAM;

    public Machine selected_machine = null;
    public Machine target_machine = null;

    void Start ()
    {
		
	}
	
	void Update ()
    {
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

                if (selected_machine == null)
                {
                    if (machine_hit.team_id == TeamHelpers.HUMAN_TEAM)
                    {
                        SelectMachine(machine_hit);
                    }
                } else {
                    if (selected_machine == machine_hit)
                    {
                        DeselectAllMachines();
                    } else {
                        SelectMachine(machine_hit);
                    }                    
                }
            }

            if (selected_machine != null && target_machine != null)
            {
                machine_controller.TryTransferBitsBetweenMachines(target_machine, selected_machine);
                DeselectAllMachines();
            }
        }
    }

    private void SelectMachine(Machine machine)
    {
        if (selected_machine == null)
        {
            selected_machine = machine;
            selected_machine.ActiveBackGround(true);
        } else {
            target_machine = machine;
            target_machine.ActiveBackGround(true);
        }
    }

    private void DeselectAllMachines()
    {
        if (selected_machine != null)
        {
            selected_machine.ActiveBackGround(false);
            selected_machine = null;
        }

        if (target_machine != null)
        {
            target_machine.ActiveBackGround(false);
            target_machine = null;
        }
    }
}
