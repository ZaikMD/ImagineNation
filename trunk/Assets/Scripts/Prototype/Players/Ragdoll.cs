using UnityEngine;
using System.Collections;

public class Ragdoll : MonoBehaviour {

	public GameObject m_GhostPrefab;
	Vector3 m_InitialPos;
	public float m_GhostRange = 0.0f;
	public float m_Speed = 0.0f;
	Vector3 m_Direction;

	// Use this for initialization
	void Start () {
	
		m_InitialPos = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {


		m_GhostPrefab.transform.position = new Vector3 (m_GhostPrefab.transform.position.x, m_GhostPrefab.transform.position.y  + m_Speed * Time.deltaTime, m_GhostPrefab.transform.position.z);


		float distance = Vector3.Distance (m_InitialPos, m_GhostPrefab.transform.position);
		if(distance > m_GhostRange)
		{
			Destroy(this.gameObject);
		}
	
	}
	
}
