using UnityEngine;
using System.Collections;

public class BaseGoal : MonoBehaviour {

	public Transform m_LevelEnd;
	public string m_NextScene;
	public GameObject m_TemporaryHidingSpot;

	//public GameObject m_Zipper;
	protected bool[] m_AtEnd = new bool[2];
	protected bool[] m_MovingToEnd = new bool[2];
	protected float m_Speed;

	CharacterController[] m_Players = new CharacterController[2];



	void Start()
	{
		for(int i = 0; i < m_Players.Length; i++)
		{
			m_AtEnd[i] = false;
			m_MovingToEnd[i] = false;
		}

	m_Speed = GameObject.FindGameObjectWithTag ("Player").GetComponent<BaseMovementAbility> ().m_GroundSpeed * Time.deltaTime;
	
	}

	void Update()
	{
	
	for(int i = 0; i < m_Players.Length; i++)
	{
		if(m_MovingToEnd [i])
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
		//Goatse.bz is the shit



		//GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		//foreach(GameObject player in players)
		//{

		//must create player prefabs that have animations set to them and move the actual player hidden from the camera
		for (int i = 0; i < m_Players.Length; i++)
		{
			if (m_Players[i] != null)
			{
			
				m_Players[i].transform.LookAt(m_LevelEnd);
				Vector3 vect3 = new Vector3(0, 0, 0);


				BaseMovementAbility tempPlayerMovement = m_Players[i].GetComponent<BaseMovementAbility>();

				//tempPlayerMovement.m_Anim.Play("Run");
	            

	            m_Players[i].SimpleMove(vect3);
				m_Players[i].Move( m_Players[i].transform.forward * m_Speed);

				float distToFin = Vector3.Distance(m_Players[i].transform.position, m_LevelEnd.position);
			
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
				m_MovingToEnd[0] = true;
			}

			else 
			{
				m_Players[1] = (CharacterController)other.GetComponent(typeof (CharacterController));
				m_MovingToEnd[1] = true;
			}
		}



	}

	public void LoadNext()
	{
		//Tell Game Data Stuff
		Application.LoadLevel (m_NextScene);
	}


}
