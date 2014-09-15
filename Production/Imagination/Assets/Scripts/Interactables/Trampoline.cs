using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public GameObject m_JumpGameObject;

	Vector3 m_TrampolinePosition = Vector3.zero;
	Vector3 m_LaunchDirection = Vector3.zero;
	//Vector3 m_JumpPosition = Vector3.zero;

	float m_VerticalVelocity;


	// Use this for initialization
	void Start () {

		if (m_JumpGameObject != null)
		{
			
		
			m_TrampolinePosition = gameObject.transform.position;
			Debug.Log (m_TrampolinePosition);
			//m_JumpGameObject.transform.position; //gameObject.transform.position;
			Debug.Log (m_JumpGameObject.transform.position);

			//Vector3 m_LaunchDirection = new Vector3 ((m_JumpGameObject.transform.position.x - m_TrampolinePosition.x), (m_JumpGameObject.transform.position.y - m_TrampolinePosition.y), 
			//                                         (m_JumpGameObject.transform.position.z - m_TrampolinePosition.z));

			 m_LaunchDirection =  m_JumpGameObject.transform.position - m_TrampolinePosition;
			Debug.Log (m_LaunchDirection);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if (m_JumpGameObject != null)
		{

		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (m_JumpGameObject != null)
		{
			if (other.tag == "Player")
			{

			}
		}

	}
}
