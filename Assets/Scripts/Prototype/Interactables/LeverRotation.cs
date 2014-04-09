using UnityEngine;
using System.Collections;

public class LeverRotation : MonoBehaviour, Observer {
	
	//public GameObject m_RotationDummy = null;

	public Subject m_Subject;
	public float m_RotationSpeed = 0.1f;

	//public Transform m_StartPos;
	//public Transform m_EndPos;

	public Vector3 m_RotationValue = Vector3.zero;
	//private Vector3 m_OriginalRotation = Vector3.zero;

	//public Quaternion m_Rotation = Quaternion.Euler(new Vector3(0, 0, 0));

	bool m_TriggerActivated = false;
	bool m_IsPaused = false;
	bool m_JustHit = false;


	// Use this for initialization
	void Start () 
	{
	
	
		//m_OriginalRotation = this.transform.rotation;
		GameManager.Instance.addObserver (this);
		m_Subject.addObserver (this);// adds to the list


	}
	
	// Update is called once per frame
	void Update () 
	{

		if(!m_IsPaused )
		{
			if(m_TriggerActivated)
			{
				m_TriggerActivated = false;
				this.gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x + m_RotationValue.x, transform.eulerAngles.y + m_RotationValue.y, transform.eulerAngles.z + m_RotationValue.z);

			}
		}
	
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame ||recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsPaused = !m_IsPaused; //toggles if you paused
		}

		if(recievedEvent == ObeserverEvents.Used)
		{
			m_TriggerActivated = !m_TriggerActivated; //Activate the rotation
		}
	}
}
