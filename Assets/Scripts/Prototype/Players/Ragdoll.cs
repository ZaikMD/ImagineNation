using UnityEngine;
using System.Collections;

public class Ragdoll : MonoBehaviour 
{
	//used for timer
	const float TIME = 4.0f;
	float m_Timer = 0.0f;

	//ghost movement
	public GameObject m_GhostPrefab;
	Vector3 m_InitialPos;
	public float m_GhostRange = 0.0f;
	public float m_Speed = 0.0f;
	Vector3 m_Direction;

	// Use this for initialization
	void Start () {
	
		//make the initial position equal to the gameobject position that the script is attached to
		m_InitialPos = transform.position;

		// Set Timer:
		m_Timer = TIME; 
	
	}
	
	// Update is called once per frame
	void Update () {

		//if there is a ghost to attached to the gameobject
		if(m_GhostPrefab != null)
		{
		//ghost's position will go up
		m_GhostPrefab.transform.position = new Vector3 (m_GhostPrefab.transform.position.x, m_GhostPrefab.transform.position.y  + m_Speed * Time.deltaTime, m_GhostPrefab.transform.position.z);


			//create a float distance that will be used to check how far the ghost is to see if it should be deleted or not
		float distance = Vector3.Distance (m_InitialPos, m_GhostPrefab.transform.position);
			if(distance > m_GhostRange)
			{
				Destroy(this.gameObject);
			}
	
		}

		//if there are no ghosts attached
		if (m_GhostPrefab == null)
		{
			// Decrement timer
			m_Timer -= Time.deltaTime;

			//once timer = 0 delete the gameobject
			if (m_Timer <= 0)
			{
				Destroy(this.gameObject);
			}
		}
	}



}


