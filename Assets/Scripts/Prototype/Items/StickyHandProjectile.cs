/*

TO USE:

Assuming it is hooked up to the projectile it should already work.

Zoey must be tagged according to the script (currently "Zoey")

All the projectile is, is a prefab with a rigid body.

3/23/2014
	Created by Jason Hein
3/25/2014
	Disabled movement while firing.
	Currently buggy if movement is enabled while firing
	Currently buggy with going into objects with forward movement
3/25/2014
	Fixed buggy movement while firing
	Fixed projectile line while moving
	Renabled movement while firing, disabled it while launching
3/29/2014
	Now finds Zoey based on object name, instead of a tag
	Fixed sticky projectile sometimes never reaching the player
	Fixed sticky line length not scaling properly
*/


using UnityEngine;
using System.Collections;

public class StickyHandProjectile : MonoBehaviour {

	//States
	enum States
	{ 
		Extending = 0,
		Retracting, 
		Launching
	}
	States m_State; 
	
	//Speed and max distance to retract
	const float m_Speed = 0.75f; 
	public const float MAX_DISTANCE = 20.0f; 
	float m_OriginalScale;

	//Target to fire at
	Vector3 m_Target;
	Vector3 m_OriginalPosition = Vector3.zero;

	//Important Objects
	GameObject m_Zoey;
	GameObject m_ProjectileLine;

	//Movement, so we can stop the player from moving while launching
	PlayerMovement m_Movement;

	//On Tick
	void Update() 
	{ 
		if(enabled)
		{
			if (m_State == States.Extending)
			{
				extending();
			}
			else if (m_State == States.Retracting)
			{
				retracting();
			}
			else if (m_State == States.Launching)
			{
				launching();
			}

			//Update line to follow this projectile
			updateStickyLine();
		}
	} 

	//Check what the sticky hand hit
	void OnCollisionEnter(Collision other) 
	{ 
		//At Zoey while launching or retracting
		if (other.gameObject == m_Zoey && m_State != States.Extending)
		{
			//The player can now move again
			if (m_State == States.Launching)
			{
				Invoke("enableMovementAfterTimer", 0.10f);
			}

			m_ProjectileLine.SetActive(false);
			gameObject.SetActive(false);
		}
		//At Glass while extending
		else if (other.gameObject.CompareTag("Glass") && m_State == States.Extending)
		{
			m_State = States.Launching;

			//Fix projectile not hitting player
			rigidbody.velocity = Vector3.zero;
			(collider as CapsuleCollider).radius = 3.0f;

			//You cannot move while launching
			m_Movement.setCanMove(false);
		}
		//Hit something other than glass while extending
		else if (m_State == States.Extending)
		{
			m_State = States.Retracting;
		}
		//Hit while retracting does nothing
	}

	void enableMovementAfterTimer()
	{
		m_Movement.setCanMove(true);
	}

	//Retract
	void retracting() 
	{ 
		//Update the stickyhand projectile position
		transform.position -= Vector3.Normalize(transform.position - m_Zoey.transform.position) * m_Speed;
	} 

	//Launch
	void launching() 
	{ 
		//Update the stickyhand projectile position
		m_Zoey.transform.position += Vector3.Normalize(transform.position - m_Zoey.transform.position) * m_Speed;
	} 

	//Extending
	void extending() 
	{ 
		//If the projectile has gone too far, set it to retract
		if(Vector3.Distance(m_OriginalPosition, transform.position) >= MAX_DISTANCE)
		{ 
			m_State = States.Retracting;
			return;
		}

		//Update the stickyhand projectile position
		transform.position += m_Target * m_Speed;
	} 

	//Updates the position and length of the sticky line
	void updateStickyLine()
	{
		//Set lines position to between Zoey and this projectile
		m_ProjectileLine.transform.position = Vector3.Lerp (m_Zoey.transform.position, transform.position, 0.5f);

		//Angle it properly
		m_ProjectileLine.transform.LookAt (transform.position);
		m_ProjectileLine.transform.Rotate (new Vector3 (90,0,0));

		//Resize length
		m_ProjectileLine.transform.localScale = new Vector3 (m_ProjectileLine.transform.localScale.x, Vector3.Distance (m_Zoey.transform.position, transform.position) / 2.0f, m_ProjectileLine.transform.localScale.z);
	}

	/// <summary>
	/// Sets the target position to fire at, and initializes the stickyhand line object
	/// </summary>
	/// <param name="target">Target.</param>
	public void activate(Vector3 target) 
	{ 
		//Reset is to extending
		m_State = States.Extending;
		gameObject.SetActive(true);

		//Find Zoey
		if (m_Zoey == null)
		{
			m_Zoey = GameObject.Find ("Zoey");
		}

		//Get component to move the player
		if (m_Movement == null)
		{
			m_Movement = (PlayerMovement)m_Zoey.GetComponent<PlayerMovement>();
		}
		(collider as CapsuleCollider).radius = 2.0f;

		//Save position for accurate distance calculations
		m_OriginalPosition = transform.position;

		//Set initial positions and rotations
		m_Target = target;
		transform.Rotate (m_Zoey.transform.rotation.eulerAngles - transform.rotation.eulerAngles);
		transform.Rotate (new Vector3 (90,0,0));

		//Create line to trail behind
		if (m_ProjectileLine)
		{
			m_ProjectileLine.SetActive(true);

		}
		else
		{
			m_ProjectileLine = (GameObject)Instantiate(Resources.Load("StickyHandLine"), Vector3.Lerp (m_Zoey.transform.position, transform.position, 0.5f), Quaternion.identity);
		}
		updateStickyLine();
	}
}
