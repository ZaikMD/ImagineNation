using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour 
{
	public GameObject m_DebrisPrefab;
	public float m_Speed = 10.0f;
	public int m_ProjectileRange = 1;
	Vector3 m_InitialPosition;

	// Use this for initialization
	void Start () 
	{
		m_InitialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += transform.forward * m_Speed * Time.deltaTime;
		float distance = Vector3.Distance (m_InitialPosition, transform.position);
		if(distance > m_ProjectileRange)
		{
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		switch(other.gameObject.tag)
		{
		case "enemy":
			//applyDamage();
			Destroy (this.gameObject);

			break;
		case "destructableWall":
			Instantiate(m_DebrisPrefab);
			Destroy(other.gameObject);
			Destroy(this.gameObject);

			break;

		default:
			break;
		}
	}
}
