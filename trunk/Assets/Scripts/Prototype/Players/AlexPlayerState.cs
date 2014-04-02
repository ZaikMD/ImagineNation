using UnityEngine;
using System.Collections;

public class AlexPlayerState : PlayerState
{

	// Use this for initialization
    void Start()
    {

    }
	
	// Update is called once per frame
    void Update()
    {
        checkStates();   
    }
                                                                                                                                                                           
	protected override void attack()
    {
        //Call Nerf component normal shoot function.
        Debug.Log("attacking");
    }
                                                                                                                                                                                               
	protected override void aimAttack()
    {
	   //Call Nerf component Aim Shoot function.
        Debug.Log("Aim attack in progess");
    }
	protected override void  useSecondItem()
    {
	//Insert call to Matts RC car code;
        Debug.Log("using Second item");
    }

	protected override bool ableToEnterSecondItem()
    {
        // Check to see if we alex can use his rc car. 
        // returning false to actual code is implementing.
        Debug.Log("Testing to see if we can use second item");
        return false;
    }

	protected override bool getUseSecondItemInput()
	{
		return PlayerInput.Instance.getEnviromentInteraction ();
	}
}
