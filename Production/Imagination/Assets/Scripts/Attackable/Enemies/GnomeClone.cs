using UnityEngine;
using System.Collections;

/*
 * Created by Mathieu Elias
 * Date: Sept 29, 2014
 *  
 * This script handles the functionality of the Gnome Mages clones
 * 
 */
#region ChangeLog
/* 
 * 
 */
#endregion

public class GnomeClone : MonoBehaviour 
{
	private Transform m_Target;
	public GameObject m_ProjectilePrefab;
	private float m_TimeBetweenShots = 1.5f;
	private float m_ShotTimer = 0.0f;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(m_Target.position);

		m_ShotTimer -= Time.deltaTime;
		if (m_ShotTimer <= 0)
		{
			if (m_ProjectilePrefab != null)
				Instantiate (m_ProjectilePrefab, transform.position, transform.rotation);
			m_ShotTimer = m_TimeBetweenShots;
		}
	}

	public void SetTarget(Transform target)
	{
		m_Target = target;
	}
}
