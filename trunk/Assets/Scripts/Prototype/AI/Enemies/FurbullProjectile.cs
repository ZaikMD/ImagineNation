using UnityEngine;
using System.Collections;

public class FurbullProjectile : MonoBehaviour {

	Crochuck m_Spawner;
	const float FIRE_SPEED = 2.0f;

	// Use this for initialization
	public void onUse(Crochuck spawner)
	{
		m_Spawner = spawner;
		rigidbody.velocity = transform.forward + transform.up * FIRE_SPEED * m_Spawner.getDistanceToTarget();
	}
	
	void OnCollisionEnter(Collision collision)
	{
		//Do damage if we hit a player

		//FurbulProjectile

		m_Spawner.instantiateFurbull(transform.position);
		GameObject.Destroy (this.gameObject);
		Destroy (this);
	}


}
