using UnityEngine;
using System.Collections;

public class MageProjectillImpactSound : MonoBehaviour {

	SFXManager m_SFX;

	// Use this for initialization
	void Start () 
	{
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == Constants.PLAYER_STRING)
		{
			m_SFX.playSound (this.transform.position, Sounds.MageHit);
		}
	}
}
