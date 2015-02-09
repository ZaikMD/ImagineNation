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
	protected float m_SaveHitTimer = 1.0f;

	//Distance to stop moving
	const float DISTANCE_TO_STOP = 0.1f;

    //Prefab for the box
	public GameObject m_BoxPrefab;
    
    //Gravity for when the block needs to fall
	float m_Gravity = 10.0f;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	float RightAngle;
	float LeftAngle;
	float ForwardAngle;
	float BackAngle;

	// Use this for initialization
	void Start () 
	{
		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial];

		m_Respawn = transform.position;

		m_SaveHealth = m_Health;

		m_Destination = transform.position;

		m_HitTimer = m_SaveHitTimer;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

        //If the box is dead, respawn it
		if(m_Health <= 0.0f)
		{
			respawn();
		}

        //Get the character controller
		CharacterController controller = GetComponent<CharacterController>();

        //Get the direction the box is moving
		Vector3 direction = m_Destination - transform.position;

        //If the block has been hit, decrement the hit timer
		if(direction.magnitude > DISTANCE_TO_STOP)
		{
			controller.Move(direction * m_Speed * Time.deltaTime);
			m_HitTimer -= Time.deltaTime;
		}
		else
		{
			transform.position = new Vector3(m_Destination.x, transform.position.y, m_Destination.z);
		}

        //If the hit timer is less than zero, the block can be hit again
		if(m_HitTimer < 0.0f)
		{
			m_Hit = false;
			m_HitTimer = m_SaveHitTimer;
		}

        //Call the fall function
		fall ();
	}

	void respawn()
	{
        //Respawn will reset all the variables for the block and reset it's position
		m_CurrentMaterial = 0;
		transform.position = m_Respawn;
		m_Destination = m_Respawn;
		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial];
		m_Health = m_SaveHealth;
	}

    //Override the onHits 
	public override void onHit(LightCollider proj, float damage)
	{
		if(!m_Hit)
		{
			if(proj.gameObject.tag == Constants.PLAYER_PROJECTILE_STRING) //If the object is a playerProjectile, call setDestination and pass in the gameobject
			{
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
		Vector3 direction = transform.position - obj.transform.position;

		RightAngle = Vector3.Angle (direction, this.transform.right);
		LeftAngle = Vector3.Angle (direction, -this.transform.right);
		ForwardAngle = Vector3.Angle (direction, this.transform.forward);
		BackAngle = Vector3.Angle (direction, -this.transform.forward);

		float smallestAngle = RightAngle;
		string angle = Constants.RIGHT_ANGLE;

		if (LeftAngle < smallestAngle)
		{
			smallestAngle = LeftAngle;
			angle =Constants.LEFT_ANGLE;
		}
		if (ForwardAngle < smallestAngle)
		{
			smallestAngle = ForwardAngle;
			angle = Constants.FORWARD_ANGLE;
		}
		if (BackAngle < smallestAngle)
		{
			smallestAngle = BackAngle;
			angle = Constants.BACK_ANGLE;
		}

		switch (angle)
		{
		case Constants.LEFT_ANGLE:
			//Hit right side of block
			m_Destination = new Vector3(transform.position.x - m_Distance, transform.position.y, transform.position.z);
			m_Health --;
			m_CurrentMaterial ++;
			break;

		case Constants.RIGHT_ANGLE:
			//hit left side
			m_Destination = new Vector3(transform.position.x + m_Distance, transform.position.y, transform.position.z);
			m_Health --;
			m_CurrentMaterial ++;
			break;

		case Constants.BACK_ANGLE:
			//hit front side
			m_Destination = new Vector3(transform.position.x , transform.position.y, transform.position.z - m_Distance);
			m_Health --;
			m_CurrentMaterial ++;
			break;

		case Constants.FORWARD_ANGLE:
			//hit back side
			m_Destination = new Vector3(transform.position.x , transform.position.y, transform.position.z + m_Distance);
			m_Health --;
			m_CurrentMaterial ++;
			break;
		}

        //If the current material is going to be outside the array, reset it
		if(m_CurrentMaterial >= m_Materials.Length)
		{
			m_CurrentMaterial = 0;
		}

		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial]; //Set the current material
		m_Hit = true; //The block has been hit
	}

	public void setPressurePlateDestination(Vector3 destination)
	{
		m_Destination = destination; // Sets the destination that the block will go towards if it hits a pressure plate
	}

	void fall()
	{

        //Raycast downwards to see if the block will need to fall
		Vector3 rayDirection = -transform.up;
		
		Ray ray = new Ray (transform.position, rayDirection);
		
		RaycastHit rayHit;

		Physics.Raycast (ray, out rayHit, m_Gravity);

		m_Destination.y = rayHit.point.y;
	}

	protected override void onDeath ()
	{
		respawn (); //If the block dies then respawn it
	}

}
