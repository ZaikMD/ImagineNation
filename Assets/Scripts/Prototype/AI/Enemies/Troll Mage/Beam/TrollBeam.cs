using UnityEngine;
using System.Collections;

public class TrollBeam : MonoBehaviour 
{
	public float m_Speed = 0.05f;
	public const float LIFE_SPAN = 20.0f;
	float m_DeathTimer = 0;

	public GameObject m_Target;

	public bool m_IsClone = false;

	void Start()
	{
		turnIntoClone ();
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.forward = Vector3.RotateTowards (transform.forward, m_Target.transform.position - transform.position, 0.05f, 0.05f);

		transform.position += transform.forward * m_Speed;

		m_DeathTimer += Time.deltaTime;
		if(m_DeathTimer >= LIFE_SPAN)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(!m_IsClone)
		{
			if(other.tag == "Player")
			{
				other.gameObject.GetComponent<PlayerState>().FlagDamage(1);
				Destroy(this.gameObject);
			}
		}
	}

	public void turnIntoClone()
	{
		if(m_IsClone)
		{
			Collider[] colliders = (Collider[])gameObject.GetComponentsInChildren<Collider>();
			for(int i = 0; i < colliders.Length; i++)
			{
				if(colliders[i].isTrigger == false)
				{
					Destroy(colliders[i]);
				}
			}
		}
	}
}
