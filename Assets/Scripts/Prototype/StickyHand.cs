/*

TO USE:

Attach this to the player.

Zoey must be tagged according to the script (currently "Zoey")



MUST CHANGE BUTTON FOR STICKY HAND

MUST CHANGE UPDATE AND START TO REFLECT OBSERVER PATTERN

MUST CHANGE TARGET PROVIDED TO BE PROVIDED BY CAMERA


Created by Jason Hein on 3/23/2014

3/29/2014
	Now properly overrides base primary items fire function
	You can no longer fire while a sticky hand projectile is already active
*/


using UnityEngine;
using System.Collections;

public class StickyHand : BasePrimaryItem {

	GameObject m_Projectile;
	StickyHandProjectile m_ProjectileComponent;

	void Start()
	{
		//Get sticky hand projectile
		m_Projectile = (GameObject)Instantiate(Resources.Load("StickyHandProjectile"), this.transform.position + this.transform.forward, Quaternion.identity);
		m_ProjectileComponent = (StickyHandProjectile)(m_Projectile.GetComponent<StickyHandProjectile>());
		m_Projectile.SetActive (false);
	}

	//Can be removed once observer pattern is attached
	void Update() 
	{
		if (Input.GetButtonDown("Fire1"))    //Fire
		{
			//Must changed to the vector given by camera, and remove update when obsever pattern is hooked up
			fire ((transform.forward * 25) + transform.position);
		}
	}

	/// <summary>
	/// Fires the sticky hand at the specific target
	/// </summary>
	/// <param name="target">Target.</param>
	public override void fire(Vector3 target)
	{
		if (!m_Projectile.activeInHierarchy)
		{
			m_Projectile.transform.position = this.transform.position + this.transform.forward;
			m_ProjectileComponent.activate(target);
		}
	}
}
