using UnityEngine;
using System.Collections;


// TODO: comment code
// TODO: optimise the update function (remove the 2 for loops that check every point
// TODO: smooth out the movement and turnning
// TODO: Add a function to check if a player is heading off screen
// TODO: Locking certain axis to stop rotation jerking
// TODO: Track two players at once

public class RailCamera : MonoBehaviour 
{
    BezierSpline m_Rail;

	public GameObject m_Player;

	private float m_CurrentProgress;

	private Vector3[] m_Locations;
	private int m_CurrentLocation;
	private int m_GoalLocation;
	private Vector3 m_GoalPos;

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


	}
	
	// Update is called once per frame
	void Update () 
	{

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


		float curDist = Vector3.Distance (transform.position, m_Player.transform.position);
		
		float nextLoc = 0;

		for(int i = 0; i < m_Locations.Length; i++)
		{
			nextLoc = Vector3.Distance(m_Locations[i], m_Player.transform.position);
			if ( nextLoc < curDist)
			{
				if (i > 2)
				{
					m_GoalPos = m_Locations[i-2];
				}
				else 
				{
					m_GoalPos = m_Locations[i];
				}
				curDist = nextLoc;
				m_GoalLocation = i;
			}
		}

		Move ();
		transform.LookAt (m_Player.transform.position);
	}

	private void Move()
	{
		if (transform.position != m_GoalPos)
		{
			if (m_GoalLocation > m_CurrentLocation)
			transform.position = Vector3.Lerp (transform.position, m_Locations[m_CurrentLocation+1], 0.2f);

			else if (m_GoalLocation < m_CurrentLocation)
			transform.position = Vector3.Lerp (transform.position, m_Locations[m_CurrentLocation-1], 0.2f);
		}
	}
	

}
