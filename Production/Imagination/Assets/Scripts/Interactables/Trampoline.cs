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

	//Used to be able to access the trampolineJump function from BaseMovementAbility
	BaseMovementAbility m_baseMove;

	//The launch point to launch the player towards
	Transform m_LaunchPoint;
	const string LAUNCHPOINT_NAME = "JumpPoint";

	//Magnitude to launch the player
	public float LaunchAmount = 20.0f;
	public float LaunchTimer = 0.5f;

	//Load the launch point
	void Start ()
	{
		m_LaunchPoint = transform.FindChild (LAUNCHPOINT_NAME);
	}

	//When a player lands on the trampoline, launch them to the launch point
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == Constants.PLAYER_STRING)
		{
			m_baseMove = other.gameObject.GetComponent<BaseMovementAbility>();
			m_baseMove.LaunchJump((m_LaunchPoint.position - transform.position).normalized * LaunchAmount, LaunchTimer);
		}
	}
}
