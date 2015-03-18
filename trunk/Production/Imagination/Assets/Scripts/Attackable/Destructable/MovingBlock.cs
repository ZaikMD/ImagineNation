using UnityEngine;
using System.Collections;


/// <summary>
/// MovingBlock
/// 
/// Created by Zach Dubuc.
/// 
/// Moving blocks will move when a player hits them with an attack.
/// </summary>

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented and changed strings to constants- Zach Dubuc
* 28/11/2014 Edit: Removed unused variable (m_Moving, ).
* 03/12/2014 Edit: Changed Health to float value, to coincide with damage - Joe Burchill
* 04/12/2014 Edit: Changed 0 to 0.0f for new float data type.
* 04/12/2014 Edit: Made the block lerp towards its destination.
* 
*/
#endregion
[RequireComponent(typeof (CharacterController))]

public class MovingBlock : Destructable
{
    //Respawn location
	Vector3 m_Respawn;
    //Distance the block will travel each time it's hit
	public float m_Distance;
    //The array of materials that will be cycled through when the health decrements
	public Material[] m_Materials;
    //The speed the block will move
	public float m_Speed;
    //The current material
	int m_CurrentMaterial = 0;

    //The destination the block will move towards
	Vector3 m_Destination;

    //Bools
	bool m_Hit = false;

    //The health of the block
	protected float m_SaveHealth;

    //Timer for in between hits
	float m_HitTimer;

    //The reset for the hit timer
	const float HIT_TIMER = 1.0f;

	//Distance to stop moving
	const float DISTANCE_TO_STOP = 0.1f;

	//Gravity
	const float GRAVITY = 0.5f;

    //Prefab for the box
	public GameObject m_BoxPrefab;

	//Angle to use
	const float ANGLE_TO_USE = 45.01f;

	//When to pause
    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	//Chracter controller
	CharacterController m_CharacterController;


	// Use this for initialization
	void Start () 
	{
		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial];

		m_Respawn = transform.position;

		m_SaveHealth = m_Health;

		m_Destination = transform.position;

		m_HitTimer = HIT_TIMER;

		m_SFX = SFXManager.Instance;

		m_CharacterController = GetComponent<CharacterController>();
	}
	
	//Every frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

        //Get the direction the box is moving
		Vector3 direction = m_Destination - transform.position;

        //If the block has been hit, decrement the hit timer
		if(direction.magnitude > DISTANCE_TO_STOP)
		{
			//Set destination below
			fall ();

			m_CharacterController.Move(direction * m_Speed * Time.deltaTime);
			UpdateHitTimer ();
		}
		else if (m_CharacterController.enabled)
		{
			m_CharacterController.enabled = false;
			m_Hit = false;
			m_HitTimer = HIT_TIMER;
		}
	}

	//If the hit timer is less than zero, the block can be hit again
	void UpdateHitTimer ()
	{
		m_HitTimer -= Time.deltaTime;
		if(m_HitTimer < 0.0f)
		{
			m_Hit = false;
			m_HitTimer = HIT_TIMER;
		}
	}

	//Respawn the box
	void respawn()
	{
        //Respawn will reset all the variables for the block and reset it's position
		m_CurrentMaterial = 0;
		transform.position = m_Respawn;
		m_Destination = m_Respawn;
		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial];
		m_Health = m_SaveHealth;
		m_Hit = false;
		m_HitTimer = HIT_TIMER;
		m_CharacterController.enabled = false;
	}

    //Override the onHits 
	public override void onHit(LightCollider proj, float damage)
	{
		if(!m_Hit)
		{
			if(proj.gameObject.tag == Constants.PLAYER_PROJECTILE_STRING) //If the object is a playerProjectile, call setDestination and pass in the gameobject
			{
				m_SFX.playSound(transform, Sounds.LeverHit);
				setDestination (proj.gameObject);
			}
		}
		return; 
	}

	//Override the onHits 
	public override void onHit(HeavyCollider proj, float damage)
	{
		if(!m_Hit)
		{
			if(proj.gameObject.tag == Constants.PLAYER_PROJECTILE_STRING) //If the object is a playerProjectile, call setDestination and pass in the gameobject
			{
				m_SFX.playSound(transform, Sounds.LeverHit);
				setDestination (proj.gameObject);
			}
		}
		return; 
	}
	
	public override void onHit(EnemyProjectile proj)
	{
		return;
	}


    /// <summary>
    /// Set destination will raycast from obj to the blocks position. If the normal of the
    /// Transform direction matches and of the 4 directions of the blocks transform,
    /// then the destination will be set to the opposite of that direction
    /// </summary>
    /// <param name="obj"></param>
	void setDestination(GameObject obj)
	{
		//Hurt the box
		m_Health --;
		if(m_Health <= 0.0f)
		{
			respawn();
			return;
		}

		//Set the box material to a damage state
		m_CurrentMaterial++;
		if(m_CurrentMaterial >= m_Materials.Length)
		{
			m_CurrentMaterial = 0;
		}
		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial]; //Set the current material

		//The block has been hit
		m_Hit = true;
		m_CharacterController.enabled = true;

		//Direction calculation for movement
		Vector3 direction = obj.transform.forward;//(transform.position - obj.transform.position).normalized;

		//Choose a destination
		if (Vector3.Angle (direction, transform.right) < ANGLE_TO_USE)
		{
			m_Destination = transform.position + transform.right * m_Distance;
		}
		else if (Vector3.Angle (direction, -transform.right) < ANGLE_TO_USE)
		{
			m_Destination = transform.position - transform.right * m_Distance;
		}
		else if (Vector3.Angle (direction, transform.forward) < ANGLE_TO_USE)
		{
			m_Destination = transform.position + transform.forward * m_Distance;
		}
		else if (Vector3.Angle (direction, -transform.forward) < ANGLE_TO_USE)
		{
			m_Destination = transform.position - transform.forward * m_Distance;
		}
	}

	public void setPressurePlateDestination(Vector3 destination)
	{
		m_Destination = destination; // Sets the destination that the block will go towards if it hits a pressure plate
	}

	void fall()
	{
		if (!Physics.Raycast(transform.position, Vector3.down, 1.0f))
		{
			m_Destination.y = transform.position.y - GRAVITY;
		}
		else if (m_Destination.y != transform.position.y)
		{
			m_CharacterController.Move(Vector3.down);
			m_Destination.y = transform.position.y;
		}
	}

	protected override void onDeath ()
	{
		respawn (); //If the block dies then respawn it
	}

}
