using UnityEngine;
using System.Collections;

public class CrochuckBite : MonoBehaviour {

	public float m_Speed = 0.5f;

	public float LIFE_SPAN = 0.3f;
	float m_Timer = 0.0f;

	// Update is called once per frame
	void Update () 
	{
		transform.position += transform.forward * m_Speed * Time.deltaTime;
		m_Timer += Time.deltaTime;
		if(m_Timer >= LIFE_SPAN)
		{
			Destroy(this.gameObject);
			Destroy(this);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerState>().FlagDamage(1);
			Destroy(this.gameObject);
			Destroy(this);
		}
	}
}
