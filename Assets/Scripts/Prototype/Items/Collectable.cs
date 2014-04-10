/*
TO USE:

Attach this script to a light bright gameobject.

Gameobject must have a trigger collider.

Collectable inventory script must exist as a singleton.




Created by Jason Hein

*/




using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour
{
	float m_BaseYPos;
	float m_Rand;
	const float ROTATE_SPEED = 100.0f;
	Vector3 m_RotateDirection;

	void Start ()
	{
		m_BaseYPos = transform.position.y;
		m_Rand = Random.Range (0.0f, 2.0f);
		transform.Rotate (new Vector3(m_Rand * 180.0f, m_Rand * 180.0f, m_Rand * 180.0f));
		m_RotateDirection = new Vector3 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f));
	}

	void Update ()
	{
		transform.position = new Vector3 (transform.position.x, m_BaseYPos + Mathf.Sin(Time.realtimeSinceStartup + m_Rand) / 2.0f, transform.position.z);
		transform.Rotate (new Vector3(Time.deltaTime * ROTATE_SPEED * m_RotateDirection.x,
		                              Time.deltaTime * ROTATE_SPEED * m_RotateDirection.y,
		                              Time.deltaTime * ROTATE_SPEED * m_RotateDirection.z));
	}

	// Collect this Collectable
	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			CollectableInventory.Instance.collect();
			Destroy(this.gameObject);
		}
	}
}
