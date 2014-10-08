using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public GameObject m_JumpGameObject;

	Vector3 m_TrampolinePosition = Vector3.zero;
	Vector3 m_LaunchDirection = Vector3.zero;

	float m_TrampolineJump = 5.0f;

	public float JUMP_SPEED = 15.0f;

	protected const float MAX_FALL_SPEED = -15.0f;
	public float FALL_ACCELERATION = 1.0f;

	bool m_TrampolineJumpNow;

	CharacterController m_PlayerController;
	BaseMovementAbility m_baseMove;





	// Use this for initialization
	void Start () 
	{
		m_TrampolineJumpNow = false;
	}
	
	// Update is called once per frame
	void Update () 
	{	
			if (m_TrampolineJumpNow == true)
			{
				m_TrampolineJumpNow = false;
				m_PlayerController = null;
			}
	}




	void OnTriggerEnter(Collider other)
	{
			if (other.tag == "Player")
			{
				m_baseMove = other.gameObject.GetComponent<BaseMovementAbility>();
				m_baseMove.TempTrampolineJump();

			}
	
	}
	
	
}
