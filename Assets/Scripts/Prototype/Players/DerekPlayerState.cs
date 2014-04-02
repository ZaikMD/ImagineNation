using UnityEngine;
using System.Collections;

public class DerekPlayerState : PlayerState {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
       // Debug.Log("lets see if this works");
        checkStates();
    }

	protected override void attack()
    {
	    //Call Dereks attack.
    }

	protected override void aimAttack() // derek does not have aim attack;
    {
        //do nothing?
    }
    
	protected override void  useSecondItem()
    {
	    //Insert call to gregs velcro hands code;
    }

	protected override bool ableToEnterSecondItem()
    {
        throw new System.NotImplementedException();
    }

	protected override bool getUseSecondItemInput()
	{
		return PlayerInput.Instance.getEnviromentInteraction ();
	}
}
