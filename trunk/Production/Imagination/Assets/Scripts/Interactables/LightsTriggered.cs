using UnityEngine;
using System.Collections;

public class LightsTriggered : MonoBehaviour {

	public Light[] m_Lights;
	float[] m_MaxIntensity;
	bool m_Activated;
	int m_FinishedCount = 0;
	public float LightUpSpeed = 1.0f;


	// Use this for initialization
	void Start ()
	{
		//Make sure this script has light to destroy
		if (m_Lights.Length == 0)
		{
			//Remove this script
			Destroy(this);
			return;
		}

		m_MaxIntensity = new float[m_Lights.Length];
		for (int i = 0; i < m_Lights.Length; i++)
		{
			m_MaxIntensity[i] = m_Lights[i].intensity;
			m_Lights[i].enabled = false;
			m_Lights[i].intensity = 0.0f;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_Activated)
		{
			for (int i = 0; i < m_Lights.Length; i++)
			{

				if (m_Lights[i].intensity < m_MaxIntensity[i])
				{
					m_Lights[i].intensity += Time.deltaTime * (LightUpSpeed * m_MaxIntensity[i]);

					//
					if (m_Lights[i].intensity >= m_MaxIntensity[i])
					{
						m_Lights[i].intensity = m_Lights[i].intensity;
						m_FinishedCount++;
					}
				}
			}

			if (m_FinishedCount == m_Lights.Length)
			{
				Deactivate ();
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (!m_Activated && other.tag == Constants.PLAYER_STRING)
		{
			Activate ();
		}
	}

	void Activate ()
	{
		//Set to active
		m_Activated = true;

		//Turn the lights on
		for (int i = 0; i < m_Lights.Length; i++)
		{
			m_Lights[i].enabled = true;
		}
	}

	void Deactivate ()
	{
		m_Activated = false;
	}
}
