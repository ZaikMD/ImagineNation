using UnityEngine;
using System.Collections;

public class TimedDeleteSelf : MonoBehaviour 
{
	public float m_Time;

	
	// Update is called once per frame
	void Update () 
	{
		m_Time -= Time.deltaTime;
		if (m_Time <= 0.0f)
			Destroy (this.gameObject);
	}
}
