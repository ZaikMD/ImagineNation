using UnityEngine;
using System.Collections;

/*
 * Created by Mathieu Elias
 * Date: Sept 29, 2014
 *  
 * This script handles the functionality of the Gnome Mages clones
 * 
 */
#region ChangeLog
/* 
 * 
 */
#endregion

[RequireComponent(typeof(NavMeshAgent))]
public class GnomeClone : Destructable 
{
	public NavMeshAgent m_Agent;

	private Transform m_Target;
	public GameObject m_ProjectilePrefab;
	public float m_TimeBetweenShots = 1.5f;
	private float m_ShotTimer = 0.0f;


	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();
		if (m_Target != null)
		transform.LookAt(m_Target.position);

		m_ShotTimer -= Time.deltaTime;
		if (m_ShotTimer <= 0)
		{
			Shoot ();
		}
	}

	public void OnStartUp()
	{
		//Find our NavMeshAgent
		m_Agent = this.gameObject.GetComponent<NavMeshAgent>();
		
		m_Health = 1;
	}

	private void Shoot()
	{
		if (m_ProjectilePrefab != null)
			Instantiate (m_ProjectilePrefab, transform.position, transform.rotation);

		m_ShotTimer = m_TimeBetweenShots;
	}

	public void SetTarget(Transform target)
	{
		m_Target = target;
	}

	public void SetPosition(Vector3 position)
	{
		if (m_Agent != null)
		m_Agent.SetDestination (position);
	}
}
