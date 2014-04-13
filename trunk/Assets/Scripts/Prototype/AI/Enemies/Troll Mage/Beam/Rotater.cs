using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour 
{
	public float m_Amplitude = 1.0f;
	public float m_Frequency = 0.5f;
	public float m_Offset = 0.0f;
	
	float m_Timer = 0.0f;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_Timer += Time.deltaTime;

		transform.position = transform.parent.position + new Vector3 (getSinY (m_Timer), getCosY (m_Timer), 0.0f);

		GameObject newGameObject = (GameObject)Instantiate (this.gameObject, transform.position, transform.rotation);
		Destroy (newGameObject.GetComponent<Rotater> ());

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
