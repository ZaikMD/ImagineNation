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
                                                                                                                                                                           
    public override void attack()
    {
        //Call Nerf component normal shoot function.
        Debug.Log("attacking");
    }
                                                                                                                                                                                               
    public override void aimAttack()
    {
	   //Call Nerf component Aim Shoot function.
        Debug.Log("Aim attack in progess");
    }
    public override void  useSecondItem()
    {
	//Insert call to Matts RC car code;
        Debug.Log("using Second item");
    }

    public override bool ableToEnterSecondItem()
    {
        // Check to see if we alex can use his rc car. 
        // returning false to actual code is implementing.
        Debug.Log("Testing to see if we can use second item");
        return false;
    }
}
