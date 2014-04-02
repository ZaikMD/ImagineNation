using UnityEngine;
using System.Collections;

public class ZoeyPlayerState : PlayerState {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        checkStates();
	}

	protected override void attack()
    {
      //Call StickyHand component normal slap function.
    }
    
	protected override void aimAttack() 
    {
	   //Call StickyHand component Aim throw function.
    }


	protected override void  useSecondItem()
    {
	    //Insert call to Gliding code;
    }

	protected override bool ableToEnterSecondItem()
   {
       // add code to check if we can use second item
       return true;
   }

	protected override bool getUseSecondItemInput()
	{
		return PlayerInput.Instance.getJumpInput ();
	}
}
