/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
 * All Behaviours must inherit from this class
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class BaseBehaviour : MonoBehaviour 
{
	protected EnemyAI m_EnemyAI;

	protected GameObject m_Target;
	
	void Start()
	{
		m_EnemyAI = GetComponentInParent<EnemyWithMovement> ();
		start ();
	}

	protected virtual void start ()
	{

	}

	//Properties	
	public EnemyAI GetAI
	{
		get{ return m_EnemyAI;}
	}

	//Returns the navmesh agent
	public NavMeshAgent GetAgent()
	{
		// If the enemy Ai is an enemy ai with movement return the navmesh agent
		EnemyWithMovement temp = m_EnemyAI as EnemyWithMovement;
		if(temp != null)
		{
			return temp.GetAgent;
		}
		// else the enemy shouldn't have a navmesh agent so return null
		else
		{
			#if DEBUG || UNITY_EDITOR
			Debug.Log("Invalid Type");
			#endif
			return null;
		}
	}

	// Returns the path nodes used for patrolling
	public Transform[] getPathNodes()
	{
		// If the enemy ai is an enemy ai with movement return the pathnodes
		EnemyWithMovement temp = m_EnemyAI as EnemyWithMovement;		
		if(temp != null)
		{
			return temp.m_PathNodes;
		}
		// else it shouldn't have any so return null
		else
		{
			#if DEBUG || UNITY_EDITOR
			Debug.Log("Invalid Type");
			#endif
			return null;
		}
	}

	//Returns the prefab used for the combat projectile
	public GameObject getProjectilePrefab()
	{
		if (m_EnemyAI != null)
		{
			return m_EnemyAI.GetProjectilePrefab();
		}

		#if DEBUG || UNITY_EDITOR
		Debug.Log("No EnemyAI Script");
		#endif
		return null;
	}


}