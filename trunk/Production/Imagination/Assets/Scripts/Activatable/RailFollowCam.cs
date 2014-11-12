using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RailFollowCam : Activatable 
{
	public Camera m_Camera;
	public BezierSpline m_Rail;
	public float m_Time;
	public Transform m_LookTarget;

	GameObject[] m_PLayerCams;
	bool m_Active = false;
	bool m_IsDone = false;
	float m_Loc = 0.0f;



	// Use this for initialization
	void Start () 
	{
		transform.position = m_Rail.transform.position;
		transform.LookAt (m_LookTarget);

		m_Camera.enabled = false;
		m_PLayerCams = GameObject.FindGameObjectsWithTag (Constants.MAIN_CAMERA_STRING);
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!m_IsDone)
		{
			if (!m_Active)
			{

				m_Active = CheckSwitches();

				for(int i = 0; i < m_Switches.Length;i ++)
				{
					/*if(m_Switches.BeenHit() != true)
					{
						m_Active = false;
					}

					else
					{
						m_Active = true;
					} */
				}
				if (m_Active)
				{
					for (int i = 0; i < m_PLayerCams.Length; i ++)
					{
						m_PLayerCams[i].SetActive(false);
					}

					m_Camera.enabled = true;
				}
			}
			else
			{
				m_Loc += Time.deltaTime / m_Time;
				
				transform.LookAt (m_LookTarget.position);
				transform.position = m_Rail.GetPoint(m_Loc);

				if (IsDone())
				{
					m_Camera.enabled = false;

					for (int i = 0; i < m_PLayerCams.Length; i ++)
					{
						m_PLayerCams[i].SetActive(true);
					}		

				}
			}
		}
	}

	public bool IsDone()
	{
		if (m_Loc >= 1.0f)
			return true;
		else
			return false;
	}
}
