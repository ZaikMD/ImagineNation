using UnityEngine;
using System.Collections;

public class CirclingOject : MonoBehaviour 
{
	Vector3 m_Destination;

	Vector3 m_RealDestination;

	Transform m_ParentTransform;

	bool m_AtDestination = false;
	
	public void setDestination(Vector3 destination)
	{
		m_Destination = destination;
		m_AtDestination = false;
	}

	public bool getIfAtDestination()
	{
		return m_AtDestination;
	}

	void Start()
	{
		m_ParentTransform = transform.parent;
	}

	public void update () 
	{
		if(transform.parent == null)
		{
			Debug.Log("Error");
		}
		m_RealDestination = m_ParentTransform.position + m_Destination;

		if(Vector3.Distance(gameObject.transform.position, m_RealDestination) < 0.5f)
		{
			m_AtDestination = true;
		}

		gameObject.transform.position = m_ParentTransform.position + Vector3.RotateTowards(gameObject.transform.position - m_ParentTransform.position, m_Destination, 0.05f, 0.05f);
	}

	public void adjustRadius(float radius)
	{
		m_Destination.Normalize();
		m_Destination *= radius;

		transform.position = m_ParentTransform.position + ((transform.position - m_ParentTransform.position).normalized * radius);
	}
}
