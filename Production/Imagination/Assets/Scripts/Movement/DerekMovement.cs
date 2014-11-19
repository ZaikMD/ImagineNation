/*
 * Created by Joe Burchill
 * Date: Sept, 12, 2014
 *  
 * This script specifically handles the movement ability of Derek:
 * to wall jump. It checks if the player has jumped at a wall and
 * then forces them off the wall if they have jumped again while on
 * the wall. This is used with base movement and ray casting. 
 * 
 */
#region ChangeLog
/* 
 * 19/9/2014 - Changed to currectly use the new base class functionality - Jason Hein
 * 27/10/2014 - Fixed stuck on the wall bug and pre-accelerating while on the wall - Jason Hein
 * 27/10/2014 - Added getter function for jumping and falling variables - Jason Hein
 * 29/10/2014 - Added Rotation to the player when jumping off the wall - Joe Burchill
 * 14/11/2014 - Complete redesign of Derek's movement. Derek now has a grapple hook instead of double jump - Greg Fortier
 */
#endregion

using UnityEngine;
using System.Collections;

public class DerekMovement : BaseMovementAbility
{
	private const float JUMP_SPEED = 6.5f;

	public GameObject m_TempTarget;
	float m_Speed = 15.0f;

	bool m_Grapple;


    /*//Const that affect the speed of the player when on the wall
	private const float MAX_WALL_HANG = 0.4f;
	private const float WALL_JUMP_SPEED = 11.0f;
    private const float WALL_FALL_SPEED = 0.8f;
	private const float JUMP_SPEED = 6.5f;

	//Rotation angle for player
	private const float PLAYER_ANGLE_ROTATION = 180.0f;

	//Amount to send the player up off the wall
	private const float WALL_JUMP_UP_DIRECTION = 0.8f;

	//Small amount to move player off wall to cancel collision
	private const float MOVE_OFF_WALL_DIRECTION = 0.1f;

    //Timer to limit the player hanging onto the wall for too long
	private float m_WallHangTimer = 0.0f;
	const float WALL_FORCE_TIME = 0.2f;

    //Boolean to tell if the player is on the wall or not
	private bool m_OnWall = false;

    //Vector3 to track the vector that the wall raycast sends out
    Vector3 m_WallJumpDirection = Vector3.zero;
	*/

	// Use this for initialization
	void Start () 
	{ 
		m_Grapple = false;
        //Calls the base class start function
		base.start ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_TempTarget != null)
		{
			if(GetIsGrounded() == false)
			{
				if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
				{
					m_Grapple = true;
				}
			}

			if(m_Grapple)
			{
				MoveTowardsTarget();
			}

			if(Vector3.Distance(this.transform.position, m_TempTarget.transform.position) < 1.0f)
			{
				m_Grapple = false;
			}
		}


      /*  //Check if we are on the wall
        if (m_OnWall)
        {
            //if we are update the timer by time
            m_WallHangTimer += Time.deltaTime;
            //Call our overriden function for Air movement
            WallAirMovement();
			m_WallJumpDirection = RayCastReturn();
            //Check the timer against our max time on the wall
            if (m_WallHangTimer >= MAX_WALL_HANG)
            {
                //LaunchJump send player off the wall by a small amount
                //removes player from wall 
				LaunchJump(m_WallJumpDirection * MOVE_OFF_WALL_DIRECTION, WALL_FORCE_TIME);
                //Set the boolean to false
				m_OnWall = false;
            }
            else
            {
                //Check if the player hit the jump button
                if (InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
                {
                    //Run function to have the player jump off the wall
                    JumpOffWall();
                    //Set boolean to false, as we are off the wall now
                    m_OnWall = false;
                }
            }
        }
        else
        {
            //Call Base Update and reset the Wall Hang Timer
            //base.update();
			m_WallHangTimer = 0.0f;
        }
        */
		base.update();
	}

	private void MoveTowardsTarget()
	{
		Vector3 currentPosition = this.transform.position;
		Vector3 targetPosition = m_TempTarget.transform.position;

		if(Vector3.Distance(currentPosition, targetPosition) > 0.0f)
		{
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();

			this.transform.Translate(
				(directionOfTravel.x * m_Speed * Time.deltaTime),
				(directionOfTravel.y * m_Speed * Time.deltaTime),
				(directionOfTravel.z * m_Speed * Time.deltaTime),
				Space.World);

			if(m_TempTarget != null)
			{
				this.transform.eulerAngles = new Vector3 (0, m_TempTarget.gameObject.transform.eulerAngles.y,0);
				//at the end of every update i should store previous position
				//use the del, difference between previous and current position to get instaneous velocity

			}
		}

		else
		{
			m_Grapple = false;
		}


	}

/*	//Make the player fall a little while on a wall
	void WallAirMovement()
	{
		m_Velocity = new Vector3 (0.0f, -WALL_FALL_SPEED, 0.0f);
	}

	//Check if we are on a wall
    private void OnControllerColliderHit(ControllerColliderHit other)
	{
        //Checks if the gameobject we collide with is a wall and we arent grounded
		if(other.gameObject.CompareTag(Constants.WALL_TAG_STRING) && !m_CharacterController.isGrounded)
		{
            //Sets that we are on the wall
			m_OnWall = true;
		}
	}

	//Jump off the wall
	private void JumpOffWall()
	{
		LaunchJump (m_WallJumpDirection * WALL_JUMP_SPEED, WALL_FORCE_TIME);

		transform.Rotate (0.0f, PLAYER_ANGLE_ROTATION, 0.0f);
		
		//Set the player off the wall
		RequestInstantMovement(m_WallJumpDirection * WALL_JUMP_SPEED * Time.deltaTime);
	}

    private Vector3 RayCastReturn()
    {
        //Raycast towards the wall once we are on the wall, and give a vector
        //reflecting out of the wall to give us a direction to launch the 
        //Player off the wall
        RaycastHit rayHit;
        Vector3 rayOrigin = gameObject.transform.TransformDirection(Vector3.forward);
        if(Physics.Raycast(gameObject.transform.position, rayOrigin, out rayHit))
        {
            if (rayHit.collider.CompareTag(Constants.WALL_TAG_STRING))
            {
                Vector3 rayVector = rayHit.point - gameObject.transform.position;
				Vector3 reflectVector = Vector3.Reflect(rayVector, rayHit.normal);
				reflectVector.y += WALL_JUMP_UP_DIRECTION; //Reflect the direction upwards
				return reflectVector;
            }
        }
		return Vector3.zero;
    }
*/
	/// <summary>
	/// Gets the players jump speed. Must be overrided by inheriting classes in order to jump.
	/// </summary>
	protected override float GetJumpSpeed()
	{
		return JUMP_SPEED;
	}

}
