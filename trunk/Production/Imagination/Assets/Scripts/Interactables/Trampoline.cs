/*
 * Created by Greg Fortier
 * Date: Oct, 1st, 2014
 *  
 * This script checks if a player has collided with a trampoline. 
 * If he has then it will call the TrampolineJump() from BaseMovementAbility
 * 
 * 
 *08/10/2014 Edit: Commented and cleaned code - Greg Fortier
 *24/10/2014 Edit: Made it work with launching and enabled horizontal launching
 *				   Added initialization to load the launch point
*/

using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	//The launch point to launch the player towards
	const string LAUNCHPOINT_NAME = "JumpPoint";

	//Magnitude to launch the player
	public float m_LaunchAmount = 15.0f;
	public float m_LaunchTimer = 0.5f;
	Vector3 m_LaunchVelocity;

	//Load the launch point
	void Start ()
	{
		m_LaunchVelocity = (transform.FindChild (LAUNCHPOINT_NAME).position - transform.position).normalized * m_LaunchAmount;
	}

	//When a player lands on the trampoline, launch them to the launch point
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == Constants.PLAYER_STRING)
		{
			BaseMovementAbility Base = other.gameObject.GetComponent<BaseMovementAbility>();
			Base.LaunchJump(m_LaunchVelocity, m_LaunchTimer);
		}
	}
}
