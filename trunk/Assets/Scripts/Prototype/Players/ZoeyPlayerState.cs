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

   public override void attack()
    {
      //Call StickyHand component normal slap function.
    }
    
   public override void aimAttack() 
    {
	   //Call StickyHand component Aim throw function.
    }


   public override void  useSecondItem()
    {
	    //Insert call to Gliding code;
    }

   public override bool ableToEnterSecondItem()
   {
       // add code to check if we can use second item
       return false;
   }

}
