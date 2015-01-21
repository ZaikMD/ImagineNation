﻿/// <summary>
/// Base behaviour for enemies being knocked back
/// 
/// Created by Jason on Jan 16, 2015
/// 
/// </summary>

#region chagnelog
///
/// 
///
#endregion


using UnityEngine;
using System.Collections;

public class BaseKnockedBackBehavouir : BaseBehaviour {

	//Whether or not the enemy is being knocked back
	protected bool m_Active = false;

	//Velocity
	public Vector3 m_Velocity = Vector3.zero;
	const float ACCELERATION_DUE_TO_GRAVITY = 20.0f;

	//The character controller used for moving the enemy while being knocked back
	CharacterController m_Controller;

	//Amount to multiply the amount knocked back for this enemy
	protected float m_LaunchAmount = 8.0f;

	//The enemy should move upward more than they are actually launched in order to leave the ground
	protected float m_UpwardDirectionAmount = 0.5f;


	//Loads the character controller and sets it to disabled
	protected virtual void start ()
	{
		m_Controller = GetComponentInParent<CharacterController> ();
		m_Controller.enabled = false;
	}
	
	/// <summary>
	/// The enemy flies backwards
	/// </summary>
	public virtual void update ()
	{
		if (m_Active)
		{
			//Velocity affected by gravity
			m_Velocity.y -= ACCELERATION_DUE_TO_GRAVITY * Time.deltaTime;
			
			//Move the enemy by its current velocity
			m_Controller.Move (m_Velocity * Time.deltaTime);
		}
	}

	//When the ai collides wtih something while falling through the air
	public virtual void OnCollide (ControllerColliderHit hit)
	{
		if (m_Active == true)
		{
			//Ignore stopping velocity from collisions with enemies
			if (hit.gameObject.CompareTag("Enemy"))
			{
				return;
			}
			//If the enemy has collided with the kill zone
			else if (hit.gameObject.name == "Kill Zone" )
			{
				m_Active = false;
				GetComponent<Destructable>().instantKill();
			}
			//If the enemy has hit the floor
			else if (Vector3.Dot(hit.normal, Vector3.up) > 0.3f)
			{
				//Inactivate the character controller for now
				m_Controller.enabled = false;
				m_Active = false;

				//Set enemy to continue the chase
				GetAgent ().enabled = true;
				m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
			}
			//If the enemy has hit a wall or ceiling
			else if (m_Velocity.y > 0.0f)
			{
				m_Velocity.y = -0.1f;
			}
		}
	}

	/// <summary>
	/// Sets the knock back.
	/// </summary>
	public virtual void SetKnockBack (float knockBackForce, Vector3 direction)
	{
		if (m_Active)
		{
			return;
		}

		//Activate knockback in given direction
		m_Active = true;

		//Enable the character controller on the enemy
		m_Controller = GetComponentInParent<CharacterController> ();
		if (!m_Controller.enabled)
		{
			m_Controller.enabled = true;
		}

		//Turn off the nav mesh agent to avoid errors
		GetAgent ().enabled = false;

		//Move the character above the ground
		m_Controller.Move (Vector3.up);

		//Set the velocity of the enemy
		Vector2 newDirection = new Vector2 (direction.x, direction.z).normalized;
		m_Velocity = new Vector3 (knockBackForce * m_LaunchAmount * newDirection.x,
		                          knockBackForce * m_LaunchAmount * m_UpwardDirectionAmount,
		                          knockBackForce * m_LaunchAmount * newDirection.y);
	}
}