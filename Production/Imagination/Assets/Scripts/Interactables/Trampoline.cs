using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public GameObject m_JumpGameObject;

	Vector3 m_TrampolinePosition = Vector3.zero;
	Vector3 m_LaunchDirection = Vector3.zero;

	float m_TrampolineJump = 5.0f;
	float m_VerticalVelocity = 15.0f;

	protected const float MAX_FALL_SPEED = -15.0f;
	protected const float FALL_ACCELERATION = 20.0f;

	bool m_TrampolineJumpNow = false;

	CharacterController m_PlayerController;



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
			if (m_TrampolineJumpNow == true)
			{
				Launch();
			}
		}


		//	m_TrampolineJumpNow = false;

	}




	void OnTriggerEnter(Collider other)
	{
		if (m_JumpGameObject != null)
		{
			if (other.tag == "Player")
			{
				Debug.Log("JUMP");
				m_TrampolineJumpNow = true;
				m_PlayerController = (CharacterController)other.GetComponent(typeof (CharacterController));


				//Launch();


				//other.transform.position = (m_JumpGameObject.transform.position);

			}


		}

	}

	void Launch ()
	{
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{
			
			m_VerticalVelocity -= Time.deltaTime * FALL_ACCELERATION;
		}

		m_PlayerController.Move (m_LaunchDirection * (m_VerticalVelocity * m_TrampolineJump) * Time.deltaTime);


	}
	

}
