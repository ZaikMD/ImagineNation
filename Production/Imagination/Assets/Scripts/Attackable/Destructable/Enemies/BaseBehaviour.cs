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
	EnemyAI m_EnemyAI;
	
	void Start()
	{
		m_EnemyAI = GetComponentInParent<EnemyAI> ();
	}
	
	protected Vector3 getPosition()
	{
		return m_EnemyAI.transform.position;
	}
	
	protected Transform[] getPathNodes()
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