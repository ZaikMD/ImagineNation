using UnityEngine;
using System.Collections;

public class PathfindNode : MonoBehaviour 
{
	bool m_ReachedNode = false;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Enemy"))
		{
			m_ReachedNode = true;
		}
	}

	public bool getNodeStatus()
	{
		return m_ReachedNode;
	}

	public void setNodeStatus(bool reachedNode)
	{
		m_ReachedNode = reachedNode;
	}
}
