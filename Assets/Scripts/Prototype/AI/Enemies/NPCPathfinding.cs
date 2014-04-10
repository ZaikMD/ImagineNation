using UnityEngine;
using System.Collections;

public class NPCPathfinding : MonoBehaviour, Observer 
{
	Transform m_Target;
	NavMeshAgent m_Agent;

	//Set up nodes for the NPC to follow
	//Ensure all Pathnodes are empty game objects, have triggers
	//and have the pathnode script.
	public GameObject[] m_PatrolNodes;
	PathfindNode[] m_PathfindNode;
	NPC m_NPC;
	int m_MaxNodes = 0;

	bool m_MidNode = false;
	int m_NodeCount = 0;

	void Start () 
	{
		m_Agent = this.gameObject.GetComponent<NavMeshAgent> ();
		m_Agent.stoppingDistance = 0;

		m_MaxNodes = m_PatrolNodes.Length;

		m_PathfindNode = new PathfindNode[m_PatrolNodes.Length];

		for(int i = 0; i < m_PatrolNodes.Length; i++)
		{
			m_PathfindNode[i] = m_PatrolNodes[i].GetComponent<PathfindNode> ();
		}

		m_NPC = gameObject.GetComponent<NPC> ();
		if(m_NPC != null)
		{
			m_NPC.addObserver(this);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
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

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.DialogueBegin)
		{
			m_Agent.enabled = false;
		}
		else if(recievedEvent == ObeserverEvents.DialogueEnd)
		{
			m_Agent.enabled = true;
		}
	}
}
