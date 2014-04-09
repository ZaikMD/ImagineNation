//hi

using UnityEngine;
using System.Collections;

public enum EnemyPathfindingStates
{
	Pursue = 0,
	Combat,
	Patrol,
	Count,
	Unknown
}

public class EnemyPathfinding : MonoBehaviour 
{

	Transform m_Target;
	EnemyPathfindingStates m_State;
	NavMeshAgent m_Agent;
	public GameObject[] m_PatrolNodes;
	float m_InitialStoppingDistance;
	bool m_MidNode = false;
	int m_NodeCount = 0;
	public PathfindNode[] m_PathfindNode;
	public int m_MaxNodes = 3;
	GameObject[] m_Player;


	// Use this for initialization
	void Start () 
	{
		m_Agent = this.gameObject.GetComponent<NavMeshAgent> ();
		m_InitialStoppingDistance = m_Agent.stoppingDistance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(m_State)
		{
			case EnemyPathfindingStates.Pursue:
			{
				Pursue();
			}
			break;

			case EnemyPathfindingStates.Combat:
			{
				Combat();
			}
			break;

			case EnemyPathfindingStates.Patrol:
			{
				Patrol();
			}
			break;

			case EnemyPathfindingStates.Unknown:
			{
			}
			break;
		}
	
	}

	/// <summary>
	/// Pursue sets the destination of the AI
	/// </summary>
	void Pursue()
	{
		m_Agent.stoppingDistance = m_InitialStoppingDistance;
		if (m_Target != null) 
		{
			m_Agent.SetDestination(m_Target.position);
		}
	}

	/// <summary>
	/// Combat state also sets the destinaiton of the AI
	/// </summary>
	void Combat()
	{
		m_Agent.stoppingDistance = m_InitialStoppingDistance;
		if (m_Target != null) 
		{
			m_Agent.SetDestination(m_Target.position);
		}
	}

	/// <summary>
	/// Handles all the logic of the node pathfinding
	/// for the Enemy AI
	/// </summary>
	void Patrol()
	{
		m_Agent.stoppingDistance = 0;

		if(m_Target == null)
		{
			m_Target = m_PatrolNodes[m_NodeCount].transform;
		}

		if(m_PathfindNode[m_NodeCount].getNodeStatus() == true)
		{
			m_MidNode = false;
			m_NodeCount++;
		}
		else
		{
			m_MidNode = true;
			m_PathfindNode[m_NodeCount].setNodeStatus(false);
		}


		if(m_MidNode == true)
		{
			if (m_Target != null) 
			{
				m_Agent.SetDestination(m_Target.position);
			}
		}
		else
		{
			if(m_NodeCount >= m_MaxNodes)
			{
				m_NodeCount = 0;
			}
				
			m_Target = m_PatrolNodes[m_NodeCount].transform;
			m_MidNode = true;
			m_PathfindNode[m_NodeCount].setNodeStatus(false);
		}

	}

	/// <summary>
	/// Sets the state of the AI with a passed in EnemyPathfindingState
	/// </summary>
	/// <param name="nextState">Next state.</param>
	public void SetState(EnemyPathfindingStates nextState)
	{
		m_State = nextState;
	}

	/// <summary>
	/// Sets the target for the Enemy AI with a passed in GameObject
	/// </summary>
	/// <param name="nextTarget">Next target.</param>
	public void setTarget(GameObject nextTarget)
	{
		m_Target = nextTarget.transform;
	}

	/// <summary>
	/// Returns the Transfor m_Target
	/// </summary>
	/// <returns>The target.</returns>
	public Transform getTarget()
	{
		return m_Target;
	}

	//public void SetReachedNode(bool reachedNode)
	//{
	//	m_ReachedNode = reachedNode;
	//}
}
