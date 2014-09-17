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

using UnityEngine;
using System.Collections;

public class DerekMovement : BaseMovementAbility
{
    //Const that affect the speed of the player when on the wall
	private const float MAX_WALL_HANG = 2.0f;
	//private const float WALL_MAX_FALL_SPEED = -5.0f;
	private const float WALL_JUMP_SPEED_VERTICAL = 15.0f;
    private const float WALL_FALL_SPEED = -0.5f;
    private const float CAN_BE_ON_WALL_MAX = 0.75f;

    //Timer to limit the player hanging onto the wall for too long
	private float m_WallHangTimer = 0.0f;
    private float m_CanBeOnWallTimer = 0.0f;

    //Boolean to tell if the player is on the wall or not
	private bool m_OnWall = false;
    private bool m_CanBeOnWall = true;

    //Vector3 to track the vector that the wall raycast sends out
    Vector3 m_WallJumpDirection = Vector3.zero;

	// Use this for initialization
	void Start () 
	{ 
        //Calls the base class start function
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () 
	{
        //Check if we are on the wall
        if (m_OnWall)
        {
            //if we are update the timer by time
            m_WallHangTimer += Time.deltaTime;
            //Call our overriden function for Air movement
            WallAirMovement();
            //Check the timer against our max time on the wall
            if (m_WallHangTimer >= MAX_WALL_HANG)
            {
                //if true we set timer back to 0 and set the boolean to false
                m_WallHangTimer = 0.0f;
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
            base.Update();
        }
	}

	void WallAirMovement()
	{
        ////Checks if our vertical velocity is greater than our max fall speed
		//if(m_VerticalVelocity > WALL_MAX_FALL_SPEED)
		//{
        //    //Decreases our vertical velocity by Time and a const value
		//	m_VerticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
		//}
		////CharacterController move in the up direction by our vertical velocity
		//m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);


        m_VerticalVelocity = WALL_FALL_SPEED;
        m_CharacterController.Move(transform.up * m_VerticalVelocity * Time.deltaTime);
	}

    protected override void Jump()
    {
        //Runs our raycasting when the player jumps and is off the wall
        RayCastReturn(m_WallJumpDirection);
        base.Jump();
    }

    private void OnControllerColliderHit(ControllerColliderHit other)
	{
        //Checks if the gameobject we collide with is a wall and we arent grounded
		if(other.gameObject.CompareTag("Wall") && !m_CharacterController.isGrounded)
		{
            //Sets that we are on the wall
			m_OnWall = true;
		}
	}

	private void JumpOffWall()
	{
        //Sets our vertical velocity to launch the player up
		m_VerticalVelocity = WALL_JUMP_SPEED_VERTICAL;
        transform.Rotate(0, 180, 0);
		//transform.TransformDirection(m_WallJumpDirection);
	}

    private void RayCastReturn(Vector3 reflectVector)
    {
        RaycastHit rayHit;
        Vector3 rayOrigin = gameObject.transform.TransformDirection(Vector3.forward);
        if(Physics.Raycast(gameObject.transform.position, rayOrigin, out rayHit))
        {
            if (rayHit.collider.CompareTag("Wall"))
            {
                Vector3 rayVector = rayHit.point - gameObject.transform.position;
                reflectVector = Vector3.Reflect(rayVector, rayHit.normal);
            }
        }
    }
}
