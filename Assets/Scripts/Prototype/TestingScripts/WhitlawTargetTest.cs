using UnityEngine;
using System.Collections;

public class WhitlawTargetTest : MonoBehaviour, Observer 
{
	bool m_Action = false;

	public TriggerManager m_TriggerManager;

	// Use this for initialization
	void Start () 
	{
		m_TriggerManager.addObserver (this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Action == true)
		{
			transform.localScale = new Vector3 (transform.localScale.x + 1.0f, 0.1f, 1.5f);
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.Used)
		{
			Debug.Log ("Recieved All targets hit");
			m_Action = true;
		}
	}
}
