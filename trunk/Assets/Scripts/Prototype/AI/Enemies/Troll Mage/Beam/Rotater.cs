using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour 
{
	public float m_Amplitude = 1.0f;
	public float m_Frequency = 0.5f;
	public float m_Offset = 0.0f;
	
	float m_Timer = 0.0f;

	float m_SpawnTimer = 0.0f;
	const float SPAWN_DELAY = 0.2f;

	public bool m_IsClone = false;

	void Start()
	{
		if(m_IsClone)
		{
			Destroy(gameObject.GetComponent<Collider>());
		}
	}

	// Update is called once per frame
	void Update () 
	{

		m_Timer += Time.deltaTime;

		transform.position = transform.parent.position + new Vector3 (getSinY (m_Timer), getCosY (m_Timer), 0.0f);

		m_SpawnTimer += Time.deltaTime;
		if(m_SpawnTimer >= SPAWN_DELAY)
		{
			m_SpawnTimer = 0.0f;
			GameObject newGameObject = (GameObject)Instantiate (this.gameObject, transform.position, transform.rotation);
			Destroy (newGameObject.GetComponent<Rotater> ());
			Destroy (newGameObject.GetComponent<SphereCollider>());
			newGameObject.AddComponent<RotaterDelete>();
		}
	}

	float getSinY(float x)
	{
		return m_Amplitude * Mathf.Sin(m_Frequency * x + m_Offset);
	}

	float getCosY(float x)
	{
		return m_Amplitude * Mathf.Cos(m_Frequency * x + m_Offset);
	}
}
