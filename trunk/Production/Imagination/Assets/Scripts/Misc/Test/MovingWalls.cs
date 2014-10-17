using UnityEngine;
using System.Collections;

public class MovingWalls : MonoBehaviour 
{
	public GameObject m_DestinationObject;
	public float m_Speed;

	private Vector3 m_OriginalPosition;
	private Vector3 m_DestinationPosition;
	private Vector3 m_Direction;
	private bool m_MovingForward;
	private float m_PushPlayerSpeed;
	private float m_FinalPush;
	private Transform m_Other;
	private bool m_PlayerReachedLimit;

	
	void Start () 
	{
		m_OriginalPosition = transform.position;
		m_DestinationPosition = m_DestinationObject.transform.position;
		m_Direction = m_DestinationPosition - m_OriginalPosition;
		m_MovingForward = true;
		m_PushPlayerSpeed = 0.55f;
		m_FinalPush = 2.0f;
		m_PlayerReachedLimit = false;
	}

	void Update () 
	{
		Move ();

		if(m_PlayerReachedLimit && m_Other != null)
		{
			if(m_MovingForward)
			{
				m_Other.position -= m_Direction * Time.deltaTime * m_PushPlayerSpeed;
			}
			else
			{
				m_Other.position += m_Direction * Time.deltaTime * m_PushPlayerSpeed;
			}
		}

	}

	void Move ()
	{
		if(m_MovingForward)
		{
			transform.position += m_Direction * Time.deltaTime * m_Speed;
			float distance = Vector3.Distance(transform.position, m_DestinationPosition);
			if(distance < 0.1f)
			{
				m_MovingForward = false;

				if(m_Other != null)
					m_PlayerReachedLimit = true;
			}
		}
		else
		{
			transform.position -= m_Direction * Time.deltaTime * m_Speed;
			float distance = Vector3.Distance(transform.position, m_OriginalPosition);
			if(distance < 0.1f)
			{
				m_MovingForward = true;

				if(m_Other != null)
					m_PlayerReachedLimit = true;
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag == Constants.PLAYER_STRING)
		{
			m_Other = other.transform;

			if(m_MovingForward)
			{
				other.transform.position += m_Direction * Time.deltaTime * m_PushPlayerSpeed;
			}
			else
			{
				other.transform.position -= m_Direction * Time.deltaTime * m_PushPlayerSpeed;
			}

		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == Constants.PLAYER_STRING)
		{
			m_Other = null;
			m_PlayerReachedLimit = false;
		}
	}
}
