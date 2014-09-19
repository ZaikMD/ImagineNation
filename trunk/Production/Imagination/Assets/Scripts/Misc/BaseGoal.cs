using UnityEngine;
using System.Collections;

public abstract class BaseGoal : MonoBehaviour {

	public Transform m_LevelEnd;
	public string m_NextScene;
	//public GameObject m_Zipper;
	protected bool m_AtEnd = false;
	protected bool m_MovingToEnd = false;
	protected float m_Speed;

	void Start()
	{
		m_Speed = GameObject.FindGameObjectWithTag ("Player").GetComponent<BaseMovementAbility> ().m_Speed * Time.deltaTime;
	}

	void Update()
	{
	
		if(m_MovingToEnd)
		{
			MoveToEnd();
		}
		
		
		if (m_AtEnd)
		{
			LoadNext();
			return;
		}

	}


	public void MoveToEnd()
	{




		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		foreach(GameObject player in players)
		{
			player.transform.LookAt(m_LevelEnd);
			CharacterController temp = player.GetComponent<CharacterController>();
			Vector3 vect3 = new Vector3(0, 0, 0);

			BaseMovementAbility tempPlayerMovement = player.GetComponent<BaseMovementAbility>();

			tempPlayerMovement.m_CanMove = false;
			tempPlayerMovement.m_Anim.Play("Run");

			if(!tempPlayerMovement.GetIsGrounded())
			{
				tempPlayerMovement.Gravity();
			}

						//temp.SimpleMove(vect3);
			temp.Move( player.transform.forward * m_Speed);

			float distToFin = Vector3.Distance(player.transform.position, m_LevelEnd.position);
		
			if(distToFin < 0.01)
			{
				m_AtEnd = true;
			}

		}



	}

	void OnTriggerEnter(Collider Other)
	{
		if(Other.tag != "Player")
		{

		}
		m_MovingToEnd = true;

		Debug.Log("Yeah");
	}

	public abstract void LoadNext();


}
