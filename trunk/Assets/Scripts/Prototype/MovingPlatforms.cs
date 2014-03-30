using UnityEngine;
using System.Collections;

public class MovingPlatforms : MonoBehaviour 
{
	public float m_XDistance;
	public float m_YDistance;
	public float m_ZDistance;
	public float m_PauseTime;
	public float m_MoveTimeInMilliseconds;
	public bool m_NeedsSwitch;
	public bool m_MovesOnce;

	private float m_InitialPauseTime;
	private Vector3 m_DestinationPosition;
	private Vector3 m_InitialPosition;
	private bool m_HasMoved = false;

	private float m_XMovePercent;
	private float m_YMovePercent;
	private float m_ZMovePercent;
	// Use this for initialization
	void Start () 
	{
		m_InitialPauseTime = m_PauseTime;
		m_DestinationPosition = new Vector3 (transform.position.x + m_XDistance, transform.position.y + m_YDistance, transform.position.z + m_ZDistance);
		m_InitialPosition = transform.position;
	
		m_XMovePercent = m_XDistance/ m_MoveTimeInMilliseconds;
		m_YMovePercent = m_YDistance/ m_MoveTimeInMilliseconds;
		m_ZMovePercent = m_ZDistance/ m_MoveTimeInMilliseconds;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_PauseTime > 0)
		{
			m_PauseTime -= Time.deltaTime;
		}
		else if (m_HasMoved == false)
		{
			//transform.position = Vector3.Lerp(transform.position, m_LerpPosition, m_LerpTime);
			transform.Translate(m_XMovePercent, m_YMovePercent, m_ZMovePercent);

			if(transform.position == m_DestinationPosition)
			{
				m_PauseTime = m_InitialPauseTime;
				m_HasMoved = true;
			}
		}
		else if(m_HasMoved == true && m_MovesOnce == false)
		{
			//transform.position = Vector3.Lerp(transform.position, m_InitialPosition, m_LerpTime);
			transform.Translate(-1*m_XMovePercent, -1*m_YMovePercent, -1*m_ZMovePercent);

			if(transform.position == m_InitialPosition)
			{
				m_PauseTime = m_InitialPauseTime;
				m_HasMoved = false;
			}
		}



	}
}
