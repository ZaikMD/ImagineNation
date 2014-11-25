using UnityEngine;
using System.Collections;


public class RailCamera : MonoBehaviour 
{
    BezierSpline m_Rail;

	public GameObject[] m_Player;

	private float m_CurrentProgress;

	private Vector3[] m_Locations;
	private int m_CurrentLocation;
	private int m_GoalLocation;

	private Vector3 m_GoalPos;
	private Vector3 m_LookPos;

	private float m_CameraSpeed = 10.0f;

	// Use this for initialization
	void Start () 
	{
		m_Rail = GameObject.FindGameObjectWithTag ("CamRail").GetComponent<BezierSpline> ();

		m_CurrentProgress = 0f;

		m_CurrentLocation = 0;
		m_GoalLocation = 0;
		m_GoalPos = Vector3.zero;

		this.transform.position = m_Rail.gameObject.transform.position;

		m_Locations = new Vector3[100];

		for (int i = 0; i < 100; i++)
		{
			//m_Locations[i] = new Vector3();

			m_Locations[i] = m_Rail.GetPoint(m_CurrentProgress);

			m_CurrentProgress += 0.01f;

		}

		// Calculate the lookpos which should be midway between both characters
		Vector3 point1 = m_Player [0].transform.position;
		Vector3 point2 = m_Player [1].transform.position;
		
		Vector3 point3 = point2 - point1;
		point3 = point3 / 2;
		point3 += point1;
		
		m_LookPos = point3;



	}
	
	// Update is called once per frame
	void Update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}

		// Calculate the lookpos which should be midway between both characters
		Vector3 point1 = m_Player [0].transform.position;
		Vector3 point2 = m_Player [1].transform.position;
		
		Vector3 point3 = point2 - point1;
		point3 = point3 / 2;
		point3 += point1;
		
		m_LookPos = point3;


		// Find out where we are on the rail
		float prevCamDist = float.MaxValue;
		float dist = 0;

		for(int i = 0; i < m_Locations.Length; i++)
		{
			dist = Vector3.Distance(transform.position, m_Locations[i]);
			if ( dist < prevCamDist)
	  		   	{
					prevCamDist = dist;
					m_CurrentLocation = i;
     			}
		}


		// Decided where the closest point to the player is
		float curDist = Vector3.Distance (transform.position,m_LookPos);
		
		float nextLoc = 0;

		for(int i = 0; i < m_Locations.Length; i++)
		{
			nextLoc = Vector3.Distance(m_Locations[i], m_LookPos);
			if ( nextLoc < curDist)
			{
				if (nextLoc < 100f);
				{
					m_GoalPos = m_Locations[i];
				}
				curDist = nextLoc;
				m_GoalLocation = i;
			}
		}

		Move ();

		Quaternion targetRotation = Quaternion.LookRotation(m_LookPos - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 2f);
		//transform.LookAt (m_LookPos, Vector3.up);
	}

	/// <summary>
	/// Moves the camera
	/// </summary>
	private void Move()
	{
		//Are we at our destination

		if (transform.position != m_GoalPos)
		{
			// Do we have to move forward or backwards
			if (m_GoalLocation > m_CurrentLocation)
			{
			//transform.position = Vector3.Lerp (transform.position, m_Locations[m_CurrentLocation+1], 0.2f);

			Vector3 MoveDir = m_Locations[m_CurrentLocation+1] - transform.position;
			transform.position += MoveDir.normalized * m_CameraSpeed * Time.deltaTime;
			}
			else if (m_GoalLocation < m_CurrentLocation)
			{
			//transform.position = Vector3.Lerp (transform.position, m_Locations[m_CurrentLocation-1], 0.2f);
			Vector3 MoveDir = m_Locations[m_CurrentLocation-1] - transform.position;
			transform.position += MoveDir.normalized * m_CameraSpeed * Time.deltaTime;
			}
		}
	}
	

}
