using UnityEngine;
using System.Collections;

public enum PlayerPathfindingStates
{
	Following = 0,
	Combat,
	Puzzle,
	Count,
	Unknown
}

public class PlayerPathfinding : MonoBehaviour 
{

	public Transform m_Target;

	PlayerPathfindingStates m_State;
	
	NavMeshAgent m_Agent;

	//Test, possibly make public to tailor to specific siblings
	float m_MinimumDistanceAway = 10.0f;

	// Use this for initialization
	void Start () 
	{
		m_Agent = this.gameObject.GetComponent<NavMeshAgent> ();

		//TestLine
		m_State = PlayerPathfindingStates.Following;
	}

	// Update is called once per frame
	void Update () 
	{		


		switch(m_State)
		{
		case PlayerPathfindingStates.Following:
		{
			Following();
		}
			break;

		case PlayerPathfindingStates.Combat:
		{
			Combat();
		}
			break;

		case PlayerPathfindingStates.Puzzle:
		{
			Puzzle();
		}
			break;

		case PlayerPathfindingStates.Unknown:
		{
			
		}
			break;

		}
	}


	 void Following()
	{
		if (m_Target != null) 
		{
			m_Agent.enabled = true;
			m_Agent.SetDestination(m_Target.position);
		}
	}

	 void Combat()
	{
		if (m_Target != null) 
		{
			m_Agent.enabled = true;
			
			Vector3 Direction = (transform.position - m_Target.transform.position).normalized;
			m_Agent.SetDestination(m_Target.transform.position + (Direction * m_MinimumDistanceAway));
		}
	}

	public void Puzzle()
	{
		m_Agent.enabled = false;
	}

	public void SetState(PlayerPathfindingStates nextState)
	{
		m_State = nextState;
	}

	public void setTarget(GameObject nextTarget)
	{
		m_Target = nextTarget.transform;
	}

}
