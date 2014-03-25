/*

TO USE:

Attach this to the player.

Zoey must be tagged according to the script (currently "Zoey")



MUST CHANGE BUTTON FOR STICKY HAND

MUST CHANGE UPDATE AND START TO REFLECT OBSERVER PATTERN

MUST CHANGE TARGET PROVIDED TO BE PROVIDED BY CAMERA


Created by Jason Hein on 3/23/2014

*/


using UnityEngine;
using System.Collections;

public class StickyHand : MonoBehaviour {

	GameObject m_Projectile;
	
	void Start()
	{

	}

	//Can be removed once observer pattern is attached
	void Update() 
	{
		if (Input.GetButtonDown("Fire1"))    //Fire
		{
			//Must changed to the vector given by camera, and remove update when obsever pattern is hooked up
			Use ((transform.forward * 20) + transform.position);
		}
	}

	public void Use(Vector3 target)
	{
		if (m_Projectile == null)
		{
			m_Projectile = (GameObject)Instantiate(Resources.Load("StickyHandProjectile"), this.transform.position + this.transform.forward, Quaternion.identity);
			StickyHandProjectile projectile = (StickyHandProjectile)(m_Projectile.GetComponent<StickyHandProjectile>());
			projectile.updateTarget(target);
		}
	}
}
