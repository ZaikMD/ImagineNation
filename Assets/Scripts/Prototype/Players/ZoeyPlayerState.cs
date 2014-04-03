using UnityEngine;
using System.Collections;

public class ZoeyPlayerState : PlayerState {

	Cape m_Cape;

	// Use this for initialization
	void Start () 
	{
		m_Cape = gameObject.GetComponent<Cape> ();
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
		m_Cape.StartGliding ();
    }

	protected override bool ableToEnterSecondItem()
   {
       // add code to check if we can use second item
		return m_Cape.ableToBeUsed ();
   }

	protected override bool getUseSecondItemInput()
	{
		return PlayerInput.Instance.getJumpInput ();
	}
}
