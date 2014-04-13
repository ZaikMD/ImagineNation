using UnityEngine;
using System.Collections;

public class ZoeyPlayerState : PlayerState {

	Cape m_Cape;
	StickyHand m_StickyHand;

	// Use this for initialization
	protected override void start ()
	{
		m_Cape = gameObject.GetComponent<Cape> ();
		m_StickyHand = gameObject.GetComponent<StickyHand> ();
	}
	
	// Update is called once per frame
	void Update ()
    {
        checkStates();
	}

	protected override void attack()
    {
		m_StickyHand.fire ();
    }
    
	protected override void aimAttack() 
    {
		m_StickyHand.aimFire ();
    }

	protected override void  useSecondItem()
    {
		//Nothing
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

	protected override void	enterSecond()
	{
		m_Cape.StartGliding ();
	}
}
