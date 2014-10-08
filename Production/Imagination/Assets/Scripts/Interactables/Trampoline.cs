/*
 * Created by Greg Fortier
 * Date: Oct, 1st, 2014
 *  
 * This script checks if a player has collided with a trampoline. 
 * If he has then it will call the TrampolineJump() from BaseMovementAbility
 * 
 * 
 *08/10/2014 Edit: Commented and cleaned code - Greg Fortier
*/

using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	//used to be able to access the trampolineJump function from BaseMovementAbility
	BaseMovementAbility m_baseMove;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{	

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			m_baseMove = other.gameObject.GetComponent<BaseMovementAbility>();
			m_baseMove.TrampolineJump();
		}
	}
}
