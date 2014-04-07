using UnityEngine;
using System.Collections;

public class FurbullsProjectile : MonoBehaviour {
	
	public float m_Speed = 10.0f;
	public float m_ProjectileRange = 2.0f;
	Vector3 m_InitialPosition;

	Vector3 m_Direction;
	
	// Use this for initialization
	void Start () 
	{
		m_InitialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += transform.forward * m_Speed * Time.deltaTime;
		float distance = Vector3.Distance (m_InitialPosition, transform.position);
		if(distance > m_ProjectileRange)
		{
			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Checks if the Projectile has collided
	/// with the player and is then destroyed
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			//applyDamage() to player
			Destroy (this.gameObject);
		}
	}

}
