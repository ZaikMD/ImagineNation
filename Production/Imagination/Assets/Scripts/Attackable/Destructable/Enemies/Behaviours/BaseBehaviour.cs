/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
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

	protected Vector3 getPosition()
	{
		return m_EnemyAI.transform.position;
	}

	//Properties
	public NavMeshAgent GetAgent()
	{
		EnemyWithMovement temp = m_EnemyAI as EnemyWithMovement;
		
		if(temp != null)
		{
			return temp.GetAgent;
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
			Debug.Log("Invalid Type");
			#endif
			return null;
		}
	}
	
	public EnemyAI GetAI
	{
		get{ return m_EnemyAI;}
	}

	public Transform[] getPathNodes()
	{
		EnemyWithMovement temp = m_EnemyAI as EnemyWithMovement;
		
		if(temp != null)
		{
			return temp.m_PathNodes;
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
			Debug.Log("Invalid Type");
			#endif
			return null;
		}
	}


}