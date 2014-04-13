using UnityEngine;
using System.Collections;

public class RotaterDelete : MonoBehaviour 
{
	float m_Timer = 1.0f;
	public float m_Increment = 0.005f;
	public const float DEATH_TIMER = 3.0f;

	Vector3 m_InitialScale;

	// Use this for initialization
	void Start () 
	{
		m_InitialScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_Timer -= m_Increment;

		transform.localScale = m_InitialScale * m_Timer;

		if(m_Timer <= 0.0f)
		{
			Destroy(this.gameObject);
		}
	}
}
