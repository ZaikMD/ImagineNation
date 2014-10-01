using UnityEngine;
using System.Collections;

public class GnomeClone : MonoBehaviour 
{
	private GameObject m_Target;
	public GameObject m_ProjectilePrefab;
	

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(m_Target.transform.position);

		if (m_ProjectilePrefab != null)
			Instantiate (m_ProjectilePrefab, transform.position, transform.rotation);
	}

	public void SetTarget(GameObject target)
	{
		m_Target = target;
	}
}
