using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{

	CheckpointManager m_CheckpointManager;

	// Use this for initialization
	void Start () 
	{
		m_CheckpointManager = CheckpointManager.m_Instance;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Character")
		{
			//m_CheckpointManager.setCurrentCheckPoint (this, other.gameObject.GetComponent<PlayerScript>());
		}
	}
}
