﻿using UnityEngine;
using System.Collections;

public class DerekProjectile : MonoBehaviour 
{
	public GameObject m_DebrisPrefab;
	public float m_Speed = 10.0f;
	public int m_ProjectileRange = 2;
	Vector3 m_InitialPosition;

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

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			//applyDamage();
			Destroy (this.gameObject);
		}

		if(other.gameObject.tag == "DestructibleWall")
		{
			Instantiate(m_DebrisPrefab, other.transform.position, other.transform.rotation);

			Destroy(other.gameObject);

			Destroy(this.gameObject);
	
		}
	}

}
