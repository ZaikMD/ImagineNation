using UnityEngine;
using System.Collections;

public class MovingPlatforms : MonoBehaviour 
{
	public float m_XDistance;
	public float m_YDistance;
	public float m_ZDistance;
	public float m_PauseTime;
	public float m_LerpTime;
	public bool m_NeedsSwitch;

	private float m_InitialPauseTime;
	private Vector3 m_LerpPosition;
	private Vector3 m_InitialPosition;
	private bool m_HasLerped = false;
	private Vector3 m_PaddingDistance = new Vector3(10.0f, 10.0f,10.0f);
	// Use this for initialization
	void Start () 
	{
		m_InitialPauseTime = m_PauseTime;
		m_LerpPosition = new Vector3 (transform.position.x + m_XDistance, transform.position.y + m_YDistance, transform.position.z + m_ZDistance);
		m_InitialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_PauseTime > 0)
		{
			m_PauseTime -= Time.deltaTime;
		}
		else if (m_HasLerped == false)
		{
			transform.position = Vector3.Lerp(transform.position, m_LerpPosition, m_LerpTime);

			if(transform.position == m_LerpPosition - m_PaddingDistance)
			{
				m_PauseTime = m_InitialPauseTime;
				m_HasLerped = true;
			}
		}
		else if(m_HasLerped == true)
		{
			transform.position = Vector3.Lerp(transform.position, m_InitialPosition, m_LerpTime);

			if(transform.position == m_InitialPosition - m_PaddingDistance)
			{
				m_PauseTime = m_InitialPauseTime;
				m_HasLerped = false;
			}
		}



	}
}
