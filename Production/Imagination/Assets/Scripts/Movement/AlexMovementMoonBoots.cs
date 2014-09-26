/*
 * Created by Greg
 * Date: 
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
public class AlexMovementMoonBoots : BaseMovementAbility {
	
	//Super moon boot jump speed
	private const float MOON_BOOTS_JUMP = 15.0f;

	// Initialization
	void Start () {
		base.Start ();
	}
	// Just calls the base update
	void Update ()
	{
		//then calls base Update script from BaseMovementAbility
		base.Update ();
	
	}

	//Jump higher than normal
	protected override void Jump()
	{
		m_SFX.playSound (this.gameObject, Sounds.Jump);
		base.Jump ();
		m_VerticalVelocity = MOON_BOOTS_JUMP;
	}
}
