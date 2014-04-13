using UnityEngine;
using System.Collections;

public class ObjectCircler : MonoBehaviour 
{
	CirclingOject[] m_CirclingObjects;

	public float m_Radius = 5.0f;

	// Use this for initialization
	void Start () 
	{
		m_CirclingObjects = gameObject.GetComponentsInChildren<CirclingOject>();

		for(int i = 0; i < m_CirclingObjects.Length; i++)
		{
			m_CirclingObjects[i].gameObject.transform.position = Random.onUnitSphere * m_Radius;
		}
	}

	void FixedUpdate () 
	{
		for(int i = 0; i < m_CirclingObjects.Length; i++)
		{
			if(m_CirclingObjects[i].getIfAtDestination())
			{
				m_CirclingObjects[i].setDestination(Random.onUnitSphere * m_Radius);
			}
			m_CirclingObjects[i].update();
		}
	}

	public void adjustRadius(float radius)
	{
		m_Radius = radius;
		for(int i = 0; i < m_CirclingObjects.Length; i++)
		{
			m_CirclingObjects[i].adjustRadius(radius);
		}
	}
}
