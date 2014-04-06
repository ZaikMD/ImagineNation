using UnityEngine;
using System.Collections;


//Last updated 04/06/2014








public class PlayerPathfinding : MonoBehaviour 
{
	Transform m_Target;
	NavMeshAgent m_Agent;

	// Use this for initialization
	void Start () 
	{
		m_Agent = this.gameObject.GetComponent<NavMeshAgent> ();
	}

	/// <summary>
	/// When the PlayerAI should be in the following state it will call this function
	/// </summary>
	public void Following(Transform target)
	{
		m_Target = target;

		if (m_Target != null) 
		{
			m_Agent.enabled = true;
			m_Agent.SetDestination(m_Target.position);
		}
	}

	/// <summary>
	/// When the PlayerAI should be in the combat state it will call this function
	/// and pass in the minimum distance the PlayerAI should be away from the enemy
	/// </summary>
	/// <param name="minimumDistanceAway">Minimum distance away.</param>
	public void Combat(Transform target, float minimumDistanceAway)
	{
		m_Target = target;

		if (m_Target != null) 
		{
			m_Agent.enabled = true;
			
			Vector3 Direction = (transform.position - m_Target.transform.position).normalized;
			m_Agent.SetDestination(m_Target.transform.position + (Direction * minimumDistanceAway));
		}
	}

	/// <summary>
	/// When the PlayerAI should be in the puzzle state it will call this function
	/// deactive the agent and no longer use navMesh
	/// </summary>
	public void Puzzle()
	{
		m_Agent.enabled = !m_Agent.enabled;
	}
}
