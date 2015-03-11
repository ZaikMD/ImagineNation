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
 * 
 * 28/11/2014 - Fixed commenting, made sure it was up to date - Greg Fortier
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
	private PlayerHealth m_PlayerHealth;
	private const float JUMP_SPEED = 6.5f;
	
	float m_GrappleSpeed = 15.0f;
	float m_DistBeforeFalling = 1.0f;

	//Position of the point to grapple from on the player
	Vector3 GRAPPLE_FROM_POSITION = new Vector3(0.0f, 1.5f, 0.0f);

	public Transform m_GrappleHook;



	bool m_Grappling;
	bool m_CanGrapple;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// Use this for initialization
	void Start () 
	{ 
		m_Grappling = false;
		m_target = GetComponent<Targeting>();
		m_PlayerHealth = GetComponent<PlayerHealth> ();
		m_GrappleHook.renderer.enabled = true;
       
		//Calls the base class start function
		base.start ();
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		//sets m_CanGrapple to true when the players lands on the ground, this is necessary so the player can not keep grappling without ever touching the
		//ground.
		if(GetIsGrounded())
		{
			m_CanGrapple = true;
		}

		if (m_PlayerHealth.IsDead)
		{
			m_target.SetCurrentTarget(null);
			m_Grappling = false;
			m_CanGrapple = false;
		}

		//checks if there is a target in sight
		if (m_target.GetCurrentTarget() != null && m_Grappling == false)
		{
			//checks if player is on ground, he can't grapple if on ground
			if(CanGrapple())
			{
				//Checks for input, if jump has been pressed then m_Grappling = true;
				if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom) && m_PlayerHealth.IsDead != true)
				{
					m_Grappling = true;
					m_CanGrapple = false;
					m_GrappleHook.renderer.enabled = true;
					m_CurrentTarget = m_target.GetCurrentTarget();
				}
			}
		}

		//checks the distance between the player and the target, if it's smaller than m_DistBeforeFalling, you will fall
		if (m_CurrentTarget != null)
		{
			if(Vector3.Distance(this.transform.position, m_CurrentTarget.transform.position) < m_DistBeforeFalling)
			{
				m_Grappling = false;
				m_GrappleHook.renderer.enabled = false;
			}
		}

		//if you should be grappling move to your target
		if(m_Grappling)
		{
			MoveTowardsTarget();
			SetGrappleTransform ();
			return;
		}

		//Used to make sure that the player stops trying to grapple if his target gets destroyed
		if (m_target.GetCurrentTarget() == null )
		{
			m_Grappling = false;
		}


		base.UpdateVelocity();
	}

	//checks if you can grapple
	private bool CanGrapple()
	{
		if(GetIsGrounded() == false && m_CanGrapple == true)
		{
			return true;
		}

		return false;	
	}

	//Moves you towards your target
	private void MoveTowardsTarget()
	{
		if(m_CurrentTarget == null)
		{
			m_target.SetCurrentTarget(null);
			m_Grappling = false;
			return;
		}

		//gets your position and the target's position
		Vector3 currentPosition = this.transform.position;
		Vector3 targetPosition = m_CurrentTarget.transform.position;


		// if the distance between you and your target is greater than 0
        if (targetPosition != null && Vector3.Distance(currentPosition, targetPosition) > 0.0f)
		{
			//create a vector 3 that will hold the direction you must go towards and then normalize it
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();

			//translate the position at the direction of travel times the speed and deltatime
			this.transform.Translate(
				(directionOfTravel * m_GrappleSpeed * Time.deltaTime),Space.World);


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
			m_Grappling = false;
		}


	}

	//Sets the grapple to be between the player and their target
	void SetGrappleTransform ()
	{
		Vector3 distanceHalfed = (m_CurrentTarget.transform.position - transform.position - GRAPPLE_FROM_POSITION) / 2.0f;
		m_GrappleHook.position = transform.position + GRAPPLE_FROM_POSITION + distanceHalfed;
		m_GrappleHook.localScale = new Vector3 (m_GrappleHook.localScale.x, distanceHalfed.magnitude, m_GrappleHook.localScale.z);
		m_GrappleHook.rotation = Quaternion.FromToRotation (Vector3.up, distanceHalfed.normalized);
	}

	protected override float GetJumpSpeed()
	{
		return JUMP_SPEED;
	}

	public override void CallBack(CallBackEvents callBack)
	{
		switch(callBack)
		{
		case CallBackEvents.FootStep_Derek:
			//Play footstep sound.
			m_SFX.playSound(this.transform, Sounds.Run);
			break;
		}	
	}

}
