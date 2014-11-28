/*
 * Created by Jason Hein October 31/2014
 * Designed to be an extension of the BaseEnemyAI script with a NavMash (for movement)
 */ 
#region ChangeLog
/* 
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

//Require our NavMeshAgent for EnemyAI
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyWithMovement : EnemyAI
{
	public Transform[] m_PathNodes;

	//Load a NavMeshAgent for the AI
	private NavMeshAgent m_Agent;

	void Start()
	{
		m_Agent = gameObject.GetComponent<NavMeshAgent>();
	}

	public NavMeshAgent GetAgent
	{
		return m_Agent;
	}
}
