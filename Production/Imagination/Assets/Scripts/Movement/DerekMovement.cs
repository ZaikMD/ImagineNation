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

	//public GameObject m_TempTarget;

	private Targeting m_target;

	private Quaternion m_LookRotation;
	private Vector3 m_Direction;
	
	float m_Speed = 15.0f;

	public bool m_Grapple;

	// Use this for initialization
	void Start () 
	{ 
		m_Grapple = false;
		m_target = GetComponent<Targeting>();
        //Calls the base class start function
		base.start ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_target.GetCurrentTarget() != null)
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

			if(Vector3.Distance(this.transform.position, m_target.GetCurrentTarget().transform.position) < 1.0f)
			{
				m_Grapple = false;
			}
		}
		//if derek is grappling this will stop all movement control and gravity of derek;
		if(m_Grapple == true)
		{
			return;
		}

		if(m_Grapple == true)
		{
			return;
		}

		base.update();
	}

	private void MoveTowardsTarget()
	{
		Vector3 currentPosition = this.transform.position;
		Vector3 targetPosition = m_target.GetCurrentTarget().transform.position;

		if(Vector3.Distance(currentPosition, targetPosition) > 0.0f)
		{
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();

			this.transform.Translate(
				(directionOfTravel.x * m_Speed * Time.deltaTime),
				(directionOfTravel.y * m_Speed * Time.deltaTime),
				(directionOfTravel.z * m_Speed * Time.deltaTime),
				Space.World);

			if(m_target.GetCurrentTarget() != null)
			{
				//finds the vector pointing from our position to the target
				m_Direction = (targetPosition - this.transform.position).normalized;

				//creates the rotation we need to be in to look at the target
				m_LookRotation = Quaternion.LookRotation(m_Direction);
				m_LookRotation.x = 0.0f;
				m_LookRotation.z = 0.0f;

				//rotates the player over time according to speed until we are in required rotation
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, m_LookRotation, Time.deltaTime * m_Speed);

			}
		}

		else
		{
			m_Grapple = false;
		}


	}

	protected override float GetJumpSpeed()
	{
		return JUMP_SPEED;
	}

}
