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
public class BaseEnemyWithMovement : BaseEnemyAI
{
	//Load a NavMeshAgent for the AI
	protected NavMeshAgent m_Agent;
	protected void Start ()
	{
		m_Agent = gameObject.GetComponent<NavMeshAgent>();
	}
}
