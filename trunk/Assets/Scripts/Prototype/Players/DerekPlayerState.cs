using UnityEngine;
using System.Collections;

public class DerekPlayerState : PlayerState 
{
	BoxingGloves m_BoxingGloves;
	VelcroGloves m_VelcroGloves;

	// Use this for initialization
	void Start () 
    {
		m_BoxingGloves = gameObject.GetComponent<BoxingGloves> ();
		m_VelcroGloves = gameObject.GetComponent<VelcroGloves> ();
	}
	
	// Update is called once per frame
	void Update ()
    {
       // Debug.Log("lets see if this works");
        checkStates();
    }

	protected override void attack()
    {
		m_BoxingGloves.fire ();
    }

	protected override void aimAttack() // derek does not have aim attack;
    {
		m_BoxingGloves.aimFire ();
    }
    
	protected override void  useSecondItem()
    {
    }

	protected override bool ableToEnterSecondItem()
    {
		return m_VelcroGloves.ableToBeUsed ();
    }

	protected override bool getUseSecondItemInput()
	{
		return PlayerInput.Instance.getEnviromentInteraction ();
	}

	protected override void	enterSecond()
	{
		m_VelcroGloves.onUse ();
	}
}
