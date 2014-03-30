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

    public override void attack()
    {
	    //Call Dereks attack.
    }

    public override void aimAttack() // derek does not have aim attack;
    {
        //do nothing?
    }
    
    public override void  useSecondItem()
    {
	    //Insert call to gregs velcro hands code;
    }

    public override bool ableToEnterSecondItem()
    {
        throw new System.NotImplementedException();
    }

}
