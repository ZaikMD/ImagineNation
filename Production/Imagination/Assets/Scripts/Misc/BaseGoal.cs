using UnityEngine;
using System.Collections;

public class BaseGoal : MonoBehaviour {

	public Transform m_LevelEnd;
	public string m_NextScene;

	protected bool[] m_AtEnd = new bool[2];
	protected float m_Speed;

	CharacterController[] m_Players = new CharacterController[2];
	Vector3[] m_PlayerPositionHolder = new Vector3[2];

	int m_PlayerWaitingToExit = 0;
	int m_MaxPlayersPossible = 2;

	float m_DistanceToEnd = 1.0f;
	
	void Start()
	{
		m_PlayerWaitingToExit = 0;

		for(int i = 0; i < m_Players.Length; i++)
		{
			m_AtEnd[i] = false;
		}

		m_Speed = GameObject.FindGameObjectWithTag (Constants.PLAYER_STRING).GetComponent<BaseMovementAbility> ().m_GroundSpeed * Time.deltaTime;
	}

	void Update()
	{
	
		for(int i = 0; i < m_Players.Length; i++)
		{
			if(m_PlayerWaitingToExit == m_MaxPlayersPossible)
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

	            m_Players[i].SimpleMove(vect3);

				float distToFin = Vector3.Distance(m_Players[i].transform.position, m_LevelEnd.position);

				if( distToFin > m_DistanceToEnd)
				{
					m_Players[i].Move( m_Players[i].transform.forward * m_Speed);
				}

			
				if(distToFin < m_DistanceToEnd)
				{
					m_AtEnd[i] = true;
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == Constants.PLAYER_STRING)
		{
			if (m_Players[0] == null)
			{
				m_Players[0] = (CharacterController)other.GetComponent(typeof (CharacterController));
				AddWaitingPlayer();
				m_PlayerPositionHolder[0] = m_Players[0].transform.position;

			}

			else 
			{
				m_Players[1] = (CharacterController)other.GetComponent(typeof (CharacterController));
				AddWaitingPlayer();
				m_PlayerPositionHolder[1] = m_Players[1].transform.position;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		m_PlayerWaitingToExit--;
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
