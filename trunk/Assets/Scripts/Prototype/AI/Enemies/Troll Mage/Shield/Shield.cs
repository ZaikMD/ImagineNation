using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour 
{
	GameObject[] m_Children;

	ObjectCircler m_ObjectCircler;

	public GameObject m_ColliderTexture;

	public bool m_IsClone = false;

	public void setRadius(float radius)
	{
		m_ColliderTexture.gameObject.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
		m_ObjectCircler.adjustRadius(radius);
	}

	// Use this for initialization
	void Start () 
	{
		m_ObjectCircler = gameObject.GetComponentInChildren<ObjectCircler>();
		m_Children = new GameObject[transform.childCount];
		for(int i = 0; i < transform.childCount; i++)
		{
			m_Children[i] = transform.GetChild(i).gameObject;
		}

		setRadius(m_ColliderTexture.transform.localScale.x / 2.0f);

		if(m_IsClone)
		{
			Destroy(m_ColliderTexture.GetComponent<Collider>());
		}
	}
	
	public void deActivate()
	{
		for(int i = 0; i < m_Children.Length; i++)
		{
			m_Children[i].SetActive(false);
		}
	}

	public void reActivate()
	{
		for(int i = 0; i < m_Children.Length; i++)
		{
			m_Children[i].SetActive(true);
		}
	}
}
