/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
 * All Behaviours must inherit from this class
 * 
 */

#region ChangeLog
/*
 * Removed the m_Target variable in order to switch it to the enemyAI class, in order to keep the target consistent through all behaviours - Mathieu Elias, Jan 8 2015
 */
#endregion
using UnityEngine;
using System.Collections;

public class BaseBehaviour : MonoBehaviour 
{
	protected EnemyAI m_EnemyAI;

	protected bool m_ControllerSet = false;

	public bool m_Altered = false;

	public GameObject m_ComponentsObject;

    public virtual AnimatorEnemyBase EnemyAnimator
    {
        get { return m_EnemyAI.i_Animator; }
    }

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

	// Returns the enemyAI's target
	protected GameObject getTarget()
	{
		if (m_EnemyAI != null)
		{
			return m_EnemyAI.Target;
		}

		return null;
	}

	//Sets the enemyAI's target
	protected void setTarget(GameObject target)
	{
		if (m_EnemyAI != null)
		{
		    m_EnemyAI.Target = target;
		}
	}

	public virtual void ComponentInfo(out string[] names, out BaseComponent[] components)
	{
		names = new string[1];
		components = new BaseComponent[1];

		names [0] = "null";
		components [0] = new NoMovement();
	}

	public virtual string BehaviourType()
	{
		return Constants.BASE_BEHAVIOUR_STRING;
	}

	public virtual int numbComponents()
	{
		return 0;
	}

	public virtual void SetComponents(string[] components)
	{

	}
}