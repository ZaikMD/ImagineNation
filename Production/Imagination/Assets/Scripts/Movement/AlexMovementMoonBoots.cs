/*
 * Created by Greg
 * Date: 10/9/2014
 *  
 * 
 * This class is used by Alex, giving him a much larger jump height.
 * 
 * Overrides the base jump function to use a different constant
 * 
 * 
 * 19/9/2014 - Changed to currectly use the new base class functionality - Jason Hein
 * 27/11/2014 - Added getter function for jumping and falling variables - Jason Hein
 */


using UnityEngine;
using System.Collections;

//Normal movement with a higher jump
public class AlexMovementMoonBoots : BaseMovementAbility 
{
	
	//Jump speeds
	private const float JUMP_SPEED = 8.0f;

	// Initialization
	void Start () {
		base.start ();
	}
	// Just calls the base update
	void Update ()
	{
		//then calls base Update script from BaseMovementAbility
		base.UpdateVelocity ();
	
	}

	/// <summary>
	/// Gets the players jump speed. Must be overrided by inheriting classes in order to jump.
	/// </summary>
	protected override float GetJumpSpeed()
	{
		return JUMP_SPEED;
	}
}
