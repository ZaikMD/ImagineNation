using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class EnemyLightPegSpawn : MonoBehaviour {

	Rigidbody m_RigidBody;

	float m_Timer;
	// Use this for initialization
	void Start()
	{
		m_Timer = 1.0f;

		m_RigidBody = this.gameObject.GetComponent<Rigidbody>();
		//m_RigidBody.freezeRotation = true;
	//	m_RigidBody.IsSleeping = false;
	//	m_RigidBody.isKinematic = true;

		Vector3 force = new Vector3(Random.Range(-1.0f, 1.0f), 5, Random.Range(-1.0f, 1.0f));

		force = force * Constants.LIGHT_PEG_FORCE;

		m_RigidBody.AddForce(force);
	}

	void Update()
	{
		if(m_Timer < 0)
		{
			Destroy(this);
			Destroy(m_RigidBody);
		}
		else
		{
			m_Timer -= Time.deltaTime;
			Vector3 force = new Vector3(Random.Range(0.0f, 1.0f), 1, Random.Range(0.0f, 1.0f));
			force = force * Constants.LIGHT_PEG_FORCE;
		//	m_RigidBody.AddForce(force);
		}
	}
}
