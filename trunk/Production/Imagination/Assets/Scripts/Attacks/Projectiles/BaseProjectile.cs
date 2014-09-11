using UnityEngine;
using System.Collections;
/// <summary>
/// Base projectile.
/// Created by Zach Dubuc
/// 
/// The projectile that the attacks use.
/// 
/// </summary>
public class BaseProjectile : MonoBehaviour 
{


	Vector3 m_InitialPosition;


	float m_MoveSpeed = 5;
	
	float m_Range = 1;



	// Use this for initialization
	void Start () 
	{
		m_InitialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += transform.forward * m_MoveSpeed * Time.deltaTime;

		float distance = Vector3.Distance (m_InitialPosition, transform.position);

		if(distance > m_Range)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter( Collision obj)
	{
		/*
		if(obj.gameObject.GetComponents<typeof>Attackable())
		{
			//TODO call OnHit() from the gameobject
		} */
	}
}
