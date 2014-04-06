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


	// Use this for initialization
	void Start () 
	{
		m_Agent = this.gameObject.GetComponent<NavMeshAgent> ();
		//m_PathfindNode = this.gameObject.GetComponent<PathfindNode> ();
		m_InitialStoppingDistance = m_Agent.stoppingDistance;
		//Testing purposes
		m_State = EnemyPathfindingStates.Patrol;
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

	void Pursue()
	{
		m_Agent.stoppingDistance = m_InitialStoppingDistance;
		if (m_Target != null) 
		{
			m_Agent.SetDestination(m_Target.position);
		}
	}

	void Combat()
	{
		m_Agent.stoppingDistance = m_InitialStoppingDistance;
		if (m_Target != null) 
		{
			m_Agent.SetDestination(m_Target.position);
		}
	}

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

	public void SetState(EnemyPathfindingStates nextState)
	{
		m_State = nextState;
	}

	//Set target
	public void setTarget(GameObject nextTarget)
	{
		m_Target = nextTarget.transform;
	}

	public Transform getTarget()
	{
		return m_Target;
	}

	//public void SetReachedNode(bool reachedNode)
	//{
	//	m_ReachedNode = reachedNode;
	//}
}
