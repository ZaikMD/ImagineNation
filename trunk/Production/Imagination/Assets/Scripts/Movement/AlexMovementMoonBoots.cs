using UnityEngine;
using System.Collections;

public class AlexMovementMoonBoots : BaseMovementAbility {

	private const float MOON_BOOTS_JUMP = 15.0f;
	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	
	}

	protected override void HeldAirMovement ()
	{
		base.HeldAirMovement ();
	}
}
