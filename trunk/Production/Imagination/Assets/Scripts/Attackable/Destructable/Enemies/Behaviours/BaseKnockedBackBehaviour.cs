/// <summary>
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

public class BaseKnockedBackBehavouir : BaseBehaviour 
{

	//Whether or not the enemy is being knocked back
	protected bool m_Active = false;

	//Velocity
	public Vector3 m_Velocity = Vector3.zero;
	const float ACCELERATION_DUE_TO_GRAVITY = 20.0f;

	//The character controller used for moving the enemy while being knocked back
	CharacterController m_Controller;

	//Amount to multiply the amount knocked back for this enemy
	//Should be changed in inheriting classes
	protected float m_LaunchAmount = 0.0f;

	//The enemy should move upward more than they are actually launched in order to leave the ground
	protected float m_UpwardDirectionAmount = 0.4f;

	//Collision with other enemies
	protected const float ONCOLLISION_SLOW_SELF_MULTIPLIER = 0.4f;
	protected const float ONCOLLISION_SLOW_ENEMY_MULTIPLIER = 0.6f;


	//Loads the character controller and sets it to disabled
	protected override void start ()
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
			GameObject hitObject = hit.gameObject;

			//Ignore stopping velocity from collisions with enemies
			if (hitObject.CompareTag("Enemy"))
			{
				BaseKnockedBackBehavouir knockedBack = hitObject.GetComponentInChildren<BaseKnockedBackBehavouir>();

				//Do not bother colliding with already flying enemies
				if (knockedBack == null || knockedBack.isActive())
				{
					return;
				}

				//The enemy collided with is knocked back with a slower speed
				knockedBack.SetKnockBack(m_Velocity.magnitude * ONCOLLISION_SLOW_ENEMY_MULTIPLIER / knockedBack.getLaunchMultiplier(), m_Velocity.normalized);

				//This enemy slows down after the collision
				m_Velocity *= ONCOLLISION_SLOW_SELF_MULTIPLIER;
			}
			if (hitObject.CompareTag("Player"))
			{
				//Knockback the player
				/*PlayerHealth health = hitObject.GetComponent<PlayerHealth>();

				//The enemy falls
				if (m_Velocity.y > 0.0f)
				{
					m_Velocity.y = -0.1f;
				}*/
			}
			//If the enemy has collided with the kill zone
			else if (hitObject.name == "Kill Zone" )
			{
				m_Active = false;
				GetComponentInParent<Destructable>().instantKill();
			}
			//If the enemy has hit the floor
			else if (Vector3.Dot(hit.normal, Vector3.up) > 0.4f)
			{
				if (m_Velocity.y > 0.0f)
				{
					return;
				}

				//Go up slightly to avoid falling through the floor
				m_Controller.Move(Vector3.up);

				//Inactivate the character controller for now
				m_Controller.enabled = false;
				m_Active = false;
				m_Velocity = Vector3.zero;

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
		//Do not get knocked back if already getting knocked back
		if (isActive())
		{
			return;
		}
		
		//Set us to the knocked back state
		if (m_EnemyAI.getState() != EnemyAI.EnemyState.KnockedBack)
		{
			m_EnemyAI.SetState (EnemyAI.EnemyState.KnockedBack);
		}

		//Activate knockback
		m_Active = true;

		//Enable the character controller on the enemy
		if (m_Controller != null && !m_Controller.enabled)
		{
			m_Controller.enabled = true;
		}

		//Turn off the nav mesh agent to avoid errors
		NavMeshAgent agent = GetAgent();
		if (agent != null)
		{
			agent.enabled = false;
		}

		//Set the velocity of the enemy
		m_Controller.Move(Vector3.up);
		Vector2 newDirection = new Vector2 (direction.x, direction.z).normalized;
		m_Velocity = new Vector3 (knockBackForce * m_LaunchAmount * newDirection.x,
		                          knockBackForce * m_LaunchAmount * m_UpwardDirectionAmount,
		                          knockBackForce * m_LaunchAmount * newDirection.y);
	}

	/// <summary>
	/// Gets the velocity.
	/// </summary>
	public Vector3 getVelocity()
	{
		return m_Velocity;
	}

	/// <summary>
	/// Returns if the enemy is already being knocked back
	/// </summary>
	public bool isActive()
	{
		return m_Active;
	}

	/// <summary>
	/// Gets the amount that his enemy affects it's own launch speed.
	/// </summary>
	public float getLaunchMultiplier()
	{
		return m_LaunchAmount;
	}
}
