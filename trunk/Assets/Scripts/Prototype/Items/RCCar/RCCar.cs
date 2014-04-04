using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	Vector3 m_StartingOffset = new Vector3(4,1,0);

	bool m_Interacting;
    List<InteractableBaseClass> m_InteractionsInRange = new List<InteractableBaseClass>();
	InteractableBaseClass m_CurrentInteraction;

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


	void Idle()
	{
		if (m_HasBegun)
		{
			if (PlayerInput.Instance.getEnviromentInteraction ()) 
			{
				if (m_Interacting)
				{
					ExitInteraction();
				}

				else if (m_InteractionsInRange.Count == 0)
				{
				m_State = RCCarStates.Exiting;
				}

				else 
				{
					m_CurrentInteraction = ClosestInteraction();
					m_State = RCCarStates.Interacting;
					m_Interacting = true;
				}
			}

			else if (PlayerInput.Instance.getMovementInput() != new Vector2(0,0))
			{
				m_State = RCCarStates.Moving;
			}
		}

	}

	public override void Move ()
	{
		m_RCCar.GetComponent<RCCarMovement> ().RegularMove ();
		m_State = RCCarStates.Idle;
	}

	void Interacting()
	{
		switch(m_CurrentInteraction.name)
		{
		case "Lever":
			{
			Lever lever = (Lever)m_CurrentInteraction;
			lever.toggleIsOn();
			m_Interacting = false;
			}
			break;

		case "MovingBlock":
			{
			
			}
			break;
		}

		m_State = RCCarStates.Idle;
	}

	void ExitInteraction()
	{
		switch(m_CurrentInteraction.name)
		{
		case "MovingBlock":
		{

			m_Interacting = false;
		}
			break;
		}
	}

	void Exiting()
	{
		Destroy (m_RCCar);
		m_RCCar = null;
		m_HasBegun = false;
		m_PlayerMovement.setCanMove (true);
		m_Alex.setExitingSecond (true);
		m_State = RCCarStates.Idle;

		CameraController cameraController = (GameObject.FindObjectOfType<Camera>()).GetComponent<CameraController>();
		
		if (cameraController)
		{
			cameraController.switchTo(m_Alex.transform);
		}
	}

	public void BeginRCCar()
	{
		if (m_HasBegun == false)
		{
			m_RCCar = (GameObject)Instantiate(Resources.Load("Prefabs/RCCarTest"));
			m_RCCar.GetComponent<RCCarMovement>().m_RCCarManager = this;
			m_RCCar.transform.position = m_Alex.transform.position + m_StartingOffset; 
			m_HasBegun = true;
			CameraController cameraController = (GameObject.FindObjectOfType<Camera>()).GetComponent<CameraController>();

			if (cameraController)
			{
				cameraController.switchTo(m_RCCar.transform);
			}
		}
	}

	public override bool ableToBeUsed()
	{
		if (m_PlayerMovement.IsGrounded ())
						return true;
				else 
						return false;
	}

	public void interactionInRange(InteractableBaseClass Interaction)
	{
		m_InteractionsInRange.Add (Interaction);
	}

	public void interactionOutInRange(InteractableBaseClass Interaction)
	{
		m_InteractionsInRange.Remove (Interaction);
	}

	public InteractableBaseClass ClosestInteraction()
	{
		float closestInteractableDistance = 0;
		InteractableBaseClass closestInteractable = m_InteractionsInRange[0];

		float currentInteractableDistance = 0;

		foreach (InteractableBaseClass Interaction in m_InteractionsInRange)
		{		
			currentInteractableDistance = Vector3.Distance(m_RCCar.transform.position, Interaction.transform.position);
			if (closestInteractableDistance > currentInteractableDistance)
			{
				closestInteractable = Interaction;
				closestInteractableDistance = currentInteractableDistance;
			}
			
		}
		
		return closestInteractable;
	}

}
