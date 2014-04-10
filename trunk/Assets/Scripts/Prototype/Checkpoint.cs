/// <summary>
/// 
/// ALL THE CHECKPOINTS NEED TO BE NAMED
/// CHECKPOINT1, CHECKPOINT2, CHECKPOINT3 ETC.
/// THE START NEEDS TO BE CALLED STARTPOINT.
/// 
/// Checkpoint.
/// </summary>

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
		if (other.tag == "Player")
		{
			CheckpointManager.m_Instance.m_CurrentCheckPoint = this;
			//m_CheckpointManager.setCurrentCheckPoint (this, other.gameObject.GetComponent<PlayerScript>());
			PlayerPrefs.SetString("CurrentCheckPoint", this.gameObject.name);
		}
	}
}
