using UnityEngine;
using System.Collections;

public class AlexPlayerState : PlayerState
{
	RCCar m_RCCar;
	NerfGun m_NerfGun;

	// Use this for initialization
    void Start()
    {
		m_RCCar = gameObject.GetComponent<RCCar> ();
		m_NerfGun = gameObject.GetComponentInChildren<NerfGun> ();
    }
	
	// Update is called once per frame
    void Update()
    {
        checkStates();   
    }
                                                                                                                                                                           
	protected override void attack()
    {
		m_NerfGun.fire ();
    }
                                                                                                                                                                                               
	protected override void aimAttack()
    {
		m_NerfGun.aimFire ();
    }
	protected override void  useSecondItem()
    {
	//Insert call to Matts RC car code;
        //Debug.Log("using Second item");
		m_RCCar.BeginRCCar ();
    }

	protected override bool ableToEnterSecondItem()
    {
        // Check to see if we alex can use his rc car. 
        // returning false to actual code is implementing.
        //Debug.Log("Testing to see if we can use second item");
        return m_RCCar.ableToBeUsed();
    }

	protected override bool getUseSecondItemInput()
	{
		return PlayerInput.Instance.getEnviromentInteraction ();
	}
}
