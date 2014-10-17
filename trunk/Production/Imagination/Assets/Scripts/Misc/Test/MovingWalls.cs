using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingWalls : MonoBehaviour 
{
	public GameObject m_DestinationObject;
	public float m_Speed;

	List<BaseMovementAbility> m_Players;

	private Vector3 m_OriginalPosition;
	private Vector3 m_DestinationPosition;
	private Vector3 m_Direction;
	private bool m_MovingForward;
	private float m_PushPlayerSpeed;

	
	void Start () 
	{
		m_Players = new List<BaseMovementAbility>();

		m_OriginalPosition = transform.position;
		m_DestinationPosition = m_DestinationObject.transform.position;
		m_Direction = m_DestinationPosition - m_OriginalPosition;
		m_MovingForward = true;
	}

	void Update () 
	{
		if(m_MovingForward)
		{
			Move (m_DestinationPosition);
		}
		else
		{
			Move(m_OriginalPosition);
		}
	}

	void Move (Vector3 destination)
	{
		Vector3 speed = ((destination - transform.position).normalized * Time.deltaTime * m_Speed);
		Vector3 distance = transform.position - destination;

		if(distance.magnitude > speed.magnitude)
		{
			transform.position += speed;

			if(m_MovingForward)
			{
				for(int i = 0; i < m_Players.Count; i++)
				{
					m_Players[i].requestMovement(speed);
				}
				m_Players.Clear();
			}
		}
		else
		{
			transform.position = destination;
			m_MovingForward = !m_MovingForward;

			m_Players.Clear();
		}
	}

	void OnTriggerStay(Collider other)
	{
		BaseMovementAbility player = other.gameObject.GetComponent<BaseMovementAbility> ();

		if(player != null)
		{
			if(!m_Players.Contains(player))
			{
				m_Players.Add(player);
			}
		}
	}
}
