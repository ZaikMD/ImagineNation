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
 */


using UnityEngine;
using System.Collections;

//Normal movement with a higher jump
public class AlexMovementMoonBoots : BaseMovementAbility 
{
	
	//Jump speeds
	private const float JUMP_SPEED = 8.0f;

	//Fall speed
	private const float MAX_FALL_SPEED = -15.0f;

	// Initialization
	void Start () {
		base.start ();
	}
	// Just calls the base update
	void Update ()
	{
		//then calls base Update script from BaseMovementAbility
		base.update ();
	
	}

	/// <summary>
	/// Gets the players jump speed. Must be overrided by inheriting classes in order to jump.
	/// </summary>
	protected override float getJumpSpeed()
	{
		return JUMP_SPEED;
	}
	
	/// <summary>
	/// Gets the players fall speed. Must be overrided by inheriting classes in order to fall.
	/// </summary>
	protected override float getFallSpeed()
	{
		return MAX_FALL_SPEED;
	}
}
