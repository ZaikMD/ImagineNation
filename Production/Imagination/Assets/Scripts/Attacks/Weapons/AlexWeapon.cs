using UnityEngine;
using System.Collections;

public class AlexWeapon : BaseWeapon 
{
	Transform m_Sword;

	// Use this for initialization
	void Start () 
	{
		start ();
	}	
	
	// Update is called once per frame
	void Update () 
	{
		update ();
	}

	protected override void ChargingEffect ()
	{

	}

	protected override void ChargedEffect ()
	{

	}

	protected override void RemoveChargingEffects ()
	{

	}

	protected override void AOEEffect ()
	{

	}
}
