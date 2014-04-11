using UnityEngine;
using System.Collections;

public class NerfPlatform : MonoBehaviour {
	
	float m_PlatformTimer = 30.0f;
	float m_OriginalPlatformTimer;
	float m_PlatformPercentage;
	
	string m_CollidedTag;

	// Use this for initialization
	void Start () 
	{
		m_OriginalPlatformTimer = m_PlatformTimer;
		SoundManager.Instance.playSound(Sounds.NerfGunPlatform, this.transform.position);
		//Physics.IgnoreCollision(this.gameObject.collider, 

	}
	
	// Update is called once per frame
	void Update () 
	{
		m_PlatformTimer -= Time.deltaTime;
		m_PlatformPercentage = m_PlatformTimer / m_OriginalPlatformTimer;

		transform.localScale = new Vector3 (2.5f * m_PlatformPercentage, 0.1f, 2.0f);
		
		if(m_PlatformTimer <= 0)
			Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "Alex")
		{
			Destroy(this.gameObject);
		}
	}
}
