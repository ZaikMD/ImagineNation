using UnityEngine;
using System.Collections;

enum RCCarStates
{
	Idle = 0,
	Moving,
	Interacting,
	Exiting
}

public class RCCar : SecondairyBase
{
	AlexPlayerState m_Alex = null;

	RCCarStates m_State = RCCarStates.Idle;
	GameObject m_RCCar = null;

	bool m_HasBegun = false;
	bool m_Interacting;	

	Vector3 m_StartingOffset = new Vector3(4,1,0);

	// Use this for initialization
	void Start () 
	{
		m_Alex = this.gameObject.GetComponent<AlexPlayerState> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_HasBegun)
		{
		switch(m_State)
		{
			case RCCarStates.Idle:
				{
				Idle();
				}
				break;

			case RCCarStates.Moving:
				{
				Move();
				}
				break;

			case RCCarStates.Interacting:
				{
				Interacting();
				}
				break;

			case RCCarStates.Exiting:
				{
				Exiting();
				}
				break;
		}
		}

	}


	public void BeginRCCar()
	{
		if (m_HasBegun == false)
		{
			m_RCCar = (GameObject)Instantiate(Resources.Load("Prefabs/RCCarPrefab"));
			m_RCCar.transform.position = m_Alex.transform.position + m_StartingOffset; 
			m_HasBegun = true;
		}
	}

	void Idle()
	{
		if (PlayerInput.Instance.getEnviromentInteraction () && m_HasBegun) 
		{
			m_State = RCCarStates.Exiting;
		}

		else if (PlayerInput.Instance.getMovementInput() != new Vector2(0,0))
		{
			m_State = RCCarStates.Moving;
		}

	}

	public override void Move ()
	{
		m_RCCar.GetComponent<RCCarMovement> ().RegularMove ();
		m_State = RCCarStates.Idle;
	}

	void Interacting()
	{
		m_State = RCCarStates.Idle;
	}

	void Exiting()
	{
		Destroy (m_RCCar);
		m_RCCar = null;
		m_HasBegun = false;
		m_PlayerMovement.setCanMove (true);
		m_Alex.setExitingSecond (true);
		m_State = RCCarStates.Idle;
	}

	public override bool ableToBeUsed()
	{
		if (m_PlayerMovement.IsGrounded ())
						return true;
				else 
						return false;
	}
}
