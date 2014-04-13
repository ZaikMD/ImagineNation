using UnityEngine;
using System.Collections;

public class PathfindNode : MonoBehaviour 
{
	bool m_ReachedNode = false;

	/// <summary>
	/// Called to check if the Enemy has entered the 
	/// node trigger and sets a boolean for the result
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("NPC"))
		{
			m_ReachedNode = true;
		}
	}

	/// <summary>
	/// returns the m_ReachedNode status
	/// </summary>
	/// <returns><c>true</c>, if node status was gotten, <c>false</c> otherwise.</returns>
	public bool getNodeStatus()
	{
		return m_ReachedNode;
	}

	/// <summary>
	/// Sets the node status.
	/// </summary>
	/// <param name="reachedNode">If set to <c>true</c> reached node.</param>
	public void setNodeStatus(bool reachedNode)
	{
		m_ReachedNode = reachedNode;
	}
}
