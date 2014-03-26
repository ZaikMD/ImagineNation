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
	States m_State = States.Extending; 
	
	//Speed and max distance to retract
	float m_Speed = 0.75f; 
	float m_MaxDist = 20.0f; 
	float m_OriginalScale;

	//Target to fire at
	Vector3 m_Target = Vector3.zero;

	//Important Objects
	GameObject m_Zoey;
	GameObject m_ProjectileLine;

	//Movement, so we can stop the player from moving while launching
	PlayerMovement m_Movement;

	//Initialization on startup
	void Start() 
	{ 

	}

	//On Tick
	void Update() 
	{ 
		if(enabled)
		{
			switch (m_State) 
			{
				case States.Extending:
				{
					extending(); 
				}
				break;
				case States.Retracting:
				{
					retracting(); 
				}
				break; 
				case States.Launching:
				{
					launching();  
				}
				break;
				default: 
				{
					//
				}
				break;
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
			m_Movement.setCanMove(true);

			Destroy(this.gameObject);
			Destroy(m_ProjectileLine);
			return;
		}
		//At Glass while extending
		else if (other.gameObject.CompareTag("Glass") && m_State == States.Extending)
		{
			m_State = States.Launching;

			//Fix projectile not hitting player
			rigidbody.velocity = Vector3.zero;

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

	//Retract
	void retracting() 
	{ 
		//Resize the following line
		m_ProjectileLine.transform.localScale = new Vector3 (m_ProjectileLine.transform.localScale.x, m_ProjectileLine.transform.localScale.y - (m_Speed * m_OriginalScale / 2), m_ProjectileLine.transform.localScale.z);

		//Update the stickyhand projectile position
		retractingUpdatePos(); 
	} 

	//Update the stickyhand projectile position and the position of its following line
	void retractingUpdatePos() 
	{
		transform.position += Vector3.Normalize(m_Zoey.transform.position - this.transform.position) * m_Speed;
	}

	//Launch
	void launching() 
	{ 
		//Resize the following line
		m_ProjectileLine.transform.localScale = new Vector3 (m_ProjectileLine.transform.localScale.x, m_ProjectileLine.transform.localScale.y - (m_Speed * m_OriginalScale / 2), m_ProjectileLine.transform.localScale.z);

		//Update the stickyhand projectile position
		launchingUpdatePlayerPos(); 
	} 

	//Update the stickyhand projectile position and the position of its following line
	void launchingUpdatePlayerPos() 
	{ 
		m_Zoey.transform.position += Vector3.Normalize(transform.position - m_Zoey.transform.position) * m_Speed;
	} 

	//Extending
	void extending() 
	{ 
		//If the projectile has gone too far, set it to retract
		if(Vector3.Distance(m_Zoey.transform.position, transform.position) > m_MaxDist)
		{ 
			m_State = States.Retracting;
			return;
		}

		//Otherwise resize the following line
		m_ProjectileLine.transform.localScale = new Vector3 (m_ProjectileLine.transform.localScale.x, m_ProjectileLine.transform.localScale.y + m_Speed * m_OriginalScale / 2, m_ProjectileLine.transform.localScale.z);

		//Update the stickyhand projectile position
		extendingUpdatePos();
	} 

	//Update the stickyhand projectile position and the position of its following line
	void extendingUpdatePos() 
	{ 
		transform.position += Vector3.Normalize(m_Target - transform.position) * m_Speed;
	}

	void updateStickyLine()
	{
		//Set lines position to between Zoey and this projectile
		m_ProjectileLine.transform.position = Vector3.Lerp (m_Zoey.transform.position, transform.position, 0.5f);
		m_ProjectileLine.transform.LookAt (transform.position);
		m_ProjectileLine.transform.Rotate (new Vector3 (90,0,0));
	}

	/// <summary>
	/// Sets the target position to fire at, and initializes the stickyhand line object
	/// </summary>
	/// <param name="target">Target.</param>
	public void updateTarget(Vector3 target) 
	{ 
		//Find Zoey
		m_Zoey = GameObject.FindGameObjectWithTag ("Zoey");

		//Get movement
		m_Movement = (PlayerMovement)m_Zoey.GetComponent<PlayerMovement>();    //Get component to move the player

		//Set initial positions and rotations
		m_Target = target;
		this.transform.Rotate (m_Zoey.transform.rotation.eulerAngles - transform.rotation.eulerAngles);
		this.transform.Rotate (new Vector3 (90,0,0));

		//Create line to trail behind
		m_ProjectileLine = (GameObject)Instantiate(Resources.Load("StickyHandLine"), Vector3.Lerp (m_Zoey.transform.position, transform.position, 0.5f), Quaternion.identity);
		m_ProjectileLine.transform.Rotate (transform.rotation.eulerAngles);
		m_OriginalScale = m_ProjectileLine.transform.localScale.y;
	}
}
