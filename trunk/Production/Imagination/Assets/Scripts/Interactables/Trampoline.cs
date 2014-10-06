using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public GameObject m_JumpGameObject;

	Vector3 m_TrampolinePosition = Vector3.zero;
	Vector3 m_LaunchDirection = Vector3.zero;

	float m_TrampolineJump = 5.0f;
	float m_VerticalVelocity = 1.0f;


	public float JUMP_SPEED = 15.0f;

	protected const float MAX_FALL_SPEED = -15.0f;
	public float FALL_ACCELERATION = 1.0f;

	bool m_TrampolineJumpNow = false;

	CharacterController m_PlayerController;
	BaseMovementAbility m_baseMove;





	// Use this for initialization
	void Start () 
	{
		if (m_JumpGameObject != null)
		{
			m_TrampolinePosition = gameObject.transform.position;
			m_LaunchDirection =  m_JumpGameObject.transform.position - m_TrampolinePosition;
			m_LaunchDirection.Normalize();
			m_LaunchDirection *= JUMP_SPEED;
			//m_TrampolinePosition = gameObject.transform.position;
			//Debug.Log (m_TrampolinePosition);
			//m_JumpGameObject.transform.position; //gameObject.transform.position;
			//Debug.Log (m_JumpGameObject.transform.position);

			//Vector3 m_LaunchDirection = new Vector3 ((m_JumpGameObject.transform.position.x - m_TrampolinePosition.x), (m_JumpGameObject.transform.position.y - m_TrampolinePosition.y), 
			//                                         (m_JumpGameObject.transform.position.z - m_TrampolinePosition.z));
			//Debug.Log("empty game object: " + m_JumpGameObject.transform.position);
			//Debug.Log("trampoline: " + m_TrampolinePosition);

			//m_LaunchDirection =  m_JumpGameObject.transform.position - m_TrampolinePosition;
			//m_LaunchDirection.Normalize();
			//m_LaunchDirection *= JUMP_SPEED;
			//Debug.Log (" final: " + m_LaunchDirection);

			//Debug.Log (m_LaunchDirection);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (m_JumpGameObject != null)
		{
			if (m_TrampolineJumpNow == true)
			{
				//if(!m_PlayerController.isGrounded)
				if(!m_baseMove.GetIsGrounded())
				{
					//Launch();
				}
				else
				{
					m_TrampolineJumpNow = false;
					m_PlayerController = null;

				}
			}
		}

		//if(m_PlayerController != null)
		//{
			//if (m_PlayerController.isGrounded)//TODO: this is broken
		//	{
		//		m_TrampolineJumpNow = false;
				//Debug.Log("Landed");
		//		m_PlayerController = null;
		//	}
		//}
	}




	void OnTriggerEnter(Collider other)
	{
		if (m_JumpGameObject != null)
		{
			if (other.tag == "Player")
			{
				//Debug.Log("JUMP");
				m_baseMove = other.gameObject.GetComponent<BaseMovementAbility>();
				//m_PlayerController = (CharacterController)other.GetComponent(typeof (CharacterController));
				//m_TrampolineJumpNow = true;
			//	GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SFXManager>().playSound(this.gameObject, Sounds.JumpPad);
				//m_baseMove.TrampolineJump(m_LaunchDirection);
				m_baseMove.LaunchJump(m_LaunchDirection);



				//Launch();


				//other.transform.position = (m_JumpGameObject.transform.position);

			}


		}

	}
	

/*	void Launch ()
	{
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{
			
			m_VerticalVelocity -= Time.deltaTime * FALL_ACCELERATION;
		}

		if (m_VerticalVelocity >= 0)
		{
			m_PlayerController.Move (m_LaunchDirection * (m_VerticalVelocity));
		}
			//m_PlayerController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);



	}
	*/

}
