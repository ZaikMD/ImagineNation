using UnityEngine;
using System.Collections;

public class BaseGoal : MonoBehaviour {

	//KEEP COMMENTED CODE, WILL BE USED WHEN ANIMATIONS ARE IMPLEMENTED

	public Transform m_LevelEnd;
	public string m_NextScene;
	//public GameObject m_TemporaryHidingSpot;

	//public Rigidbody m_AlexPrefab;
	//public Rigidbody m_ZoePrefab;
	//public Rigidbody m_DerekPrefab;


	//public GameObject m_Zipper;
	protected bool[] m_AtEnd = new bool[2];
	protected float m_Speed;

	CharacterController[] m_Players = new CharacterController[2];

	//private bool[] m_PlayerID = new bool[3];

	Vector3[] m_PlayerPositionHolder = new Vector3[2];

	int m_PlayerWaitingToExit = 0;



	void Start()
	{

		m_PlayerWaitingToExit = 0;

		for(int i = 0; i < m_Players.Length; i++)
		{
			m_AtEnd[i] = false;
		}

	m_Speed = GameObject.FindGameObjectWithTag ("Player").GetComponent<BaseMovementAbility> ().m_GroundSpeed * Time.deltaTime;
	
	}

	void Update()
	{
	
	for(int i = 0; i < m_Players.Length; i++)
	{
		if(m_PlayerWaitingToExit == 2)
		{
			MoveToEnd();
		}
	}
		
		
		if (m_AtEnd[1])
		{
			LoadNext();
			return;
		}

	}


	public void MoveToEnd()
	{  

		//must create player prefabs that have animations set to them and move the actual player hidden from the camera
		for (int i = 0; i < m_Players.Length; i++)
		{
			if (m_Players[i] != null)
			{
			
				m_Players[i].transform.LookAt(m_LevelEnd);
				Vector3 vect3 = new Vector3(0, 0, 0);


				//BaseMovementAbility tempPlayerMovement = m_Players[i].GetComponent<BaseMovementAbility>();

				//tempPlayerMovement.m_Anim.Play("Run");
	            

	            m_Players[i].SimpleMove(vect3);

				//m_Players[i].transform.position = m_TemporaryHidingSpot.transform.position;


				float distToFin = Vector3.Distance(m_Players[i].transform.position, m_LevelEnd.position);

				if( distToFin > 1.0)
				{
					m_Players[i].Move( m_Players[i].transform.forward * m_Speed);
				}

			
				if(distToFin < 1.0)
				{
					m_AtEnd[i] = true;
				}
			}
		}

		//}

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if (m_Players[0] == null)
			{
				m_Players[0] = (CharacterController)other.GetComponent(typeof (CharacterController));
				AddWaitingPlayer();
				Debug.Log(m_PlayerWaitingToExit);
				m_PlayerPositionHolder[0] = m_Players[0].transform.position;

			}

			else 
			{
				m_Players[1] = (CharacterController)other.GetComponent(typeof (CharacterController));
				AddWaitingPlayer();
				Debug.Log(m_PlayerWaitingToExit);
				m_PlayerPositionHolder[1] = m_Players[1].transform.position;
			}

			//Alex is ID number 0
			//if(other.name == "Alex")
			//{
			//	m_PlayerID[0] = true; // is Alex
			//}

			//Zoe is ID number 1
			//if (other.name == "Zoe")
			//{
			//	m_PlayerID[1] = true; // is Zoe
			//}

			//Derek is ID number 2
			//else 
			//{
			//	m_PlayerID[2] = true; // is Derek
			//}

		}
	}

	void OnTriggerExit(Collider other)
	{
		m_PlayerWaitingToExit--;
		Debug.Log (m_PlayerWaitingToExit);
	}

	public void LoadNext()
	{
		//Tell Game Data to load next level
		Application.LoadLevel (m_NextScene);
	}

	public void AddWaitingPlayer()
	{
		m_PlayerWaitingToExit++;
	}


}
