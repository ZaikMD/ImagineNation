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
	bool m_ReachedNode = false;
	bool m_MidNode = false;
	int m_NodeCount = 0;


	// Use this for initialization
	void Start () 
	{
		m_Agent = this.gameObject.GetComponent<NavMeshAgent> ();
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

		if(m_ReachedNode == true)
		{
			m_MidNode = false;
			m_NodeCount++;
		}
		else
		{
			m_MidNode = true;
			m_ReachedNode = false;
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
			m_Target = m_PatrolNodes[m_NodeCount].transform;
			m_MidNode = true;
		}

	}

	public void SetState(EnemyPathfindingStates nextState)
	{
		m_State = nextState;
	}
	
	public void setTarget(GameObject nextTarget)
	{
		m_Target = nextTarget.transform;
	}

	public Transform getTarget()
	{
		return m_Target;
	}

	public void SetReachedNode(bool reachedNode)
	{
		m_ReachedNode = reachedNode;
	}

	void OnTriggerEnter(Collider other)
	{

		if(other.gameObject.transform == m_Target)
		{
			m_ReachedNode = true;
		}
	}
}
