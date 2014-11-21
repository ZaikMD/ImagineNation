/*
 * Created by Greg Fortier
 * Date: Nov, 14th, 2014
 *  
 * This script specifically handles the movement ability of Derek:
 * to grapple hook. It checks if the player has hit the jump button twice
 * and transforms Derek's position to the position of the grapple point.
 * 
 */
#region ChangeLog
/* 
 * 14/11/2014 - Complete redesign of Derek's movement. Derek now has a grapple hook instead of double jump - Greg Fortier
 */
#endregion

using UnityEngine;
using System.Collections;

public class DerekMovement : BaseMovementAbility
{
	private Targeting m_target;
	private GameObject m_CurrentTarget;
	private Quaternion m_LookRotation;
	private Vector3 m_Direction;
	private const float JUMP_SPEED = 6.5f;
	
	float m_GrappleSpeed = 15.0f;
	float m_DistBeforeFalling = 1.0f;



	bool m_Grapple;

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
		//checks if there is a target in sight
		if (m_target.GetCurrentTarget() != null && m_Grapple == false)
		{
			//checks if player is on ground, he can't not grapple if on ground
			if(GetIsGrounded() == false)
			{
				//Checks for input, if jump has been pressed then m_Grapple = true;
				if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
				{
					m_Grapple = true;
					m_CurrentTarget = m_target.GetCurrentTarget();
				}
			}

			//if m_Grapple == true, then call MoveTowardsTarget()
			if(m_Grapple)
			{
				MoveTowardsTarget();
			}

			//checks the distance between the player and the target, if it's smaller than m_DistBeforeFalling, you will fall
			if(Vector3.Distance(this.transform.position, m_target.GetCurrentTarget().transform.position) < m_DistBeforeFalling)
			{
				m_Grapple = false;
			}
		}

		//checks the distance between the player and the target, if it's smaller than m_DistBeforeFalling, you will fall
		if (m_CurrentTarget != null)
		{
			if(Vector3.Distance(this.transform.position, m_CurrentTarget.transform.position) < m_DistBeforeFalling)
			{
				m_Grapple = false;
			}
		}
		if(m_Grapple)
		{
			MoveTowardsTarget();
			return;
		}

		base.update();
	}

	private void MoveTowardsTarget()
	{
		Vector3 currentPosition = this.transform.position;
		Vector3 targetPosition = m_CurrentTarget.transform.position;

		if(Vector3.Distance(currentPosition, targetPosition) > 0.0f)
		{
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();

			this.transform.Translate(
				(directionOfTravel.x * m_GrappleSpeed * Time.deltaTime),
				(directionOfTravel.y * m_GrappleSpeed * Time.deltaTime),
				(directionOfTravel.z * m_GrappleSpeed * Time.deltaTime),
				Space.World);

			if(m_CurrentTarget != null)
			{
				//finds the vector pointing from our position to the target
				m_Direction = (targetPosition - this.transform.position).normalized;

				//creates the rotation we need to be in to look at the target
				m_LookRotation = Quaternion.LookRotation(m_Direction);
				m_LookRotation.x = 0.0f;
				m_LookRotation.z = 0.0f;

				//rotates the player over time according to speed until we are in required rotation
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, m_LookRotation, Time.deltaTime * m_GrappleSpeed);

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
