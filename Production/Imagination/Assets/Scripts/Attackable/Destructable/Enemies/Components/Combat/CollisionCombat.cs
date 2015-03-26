/*
 * Created by Joe Burchill December 3/2014
 * 
 * This Combat component uses raycast to see if the enemy has run into
 * the player.
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class CollisionCombat : BaseCombat 
{
	//Takes in an Enemy Projectile
	public EnemyProjectile m_EnemyProjectile;

	//Differing values for our ray cast ray
	private Vector3 m_RayOrigin;
	private Vector3 m_RayDirection;
	private float m_RayDistance;
	private int m_LayerMask;
	private const float RAY_DISTANCE = 0.05f;

	//Player related gameObjects
	private GameObject m_PlayerCenterPoint;
	private GameObject m_Player;

	//Timer to track attack delay
	private float m_AttackTimer;
	private const float ATTACK_DELAY = 1.5f;


	public override void start (BaseBehaviour baseBehaviour)
	{
		base.start (baseBehaviour);

		m_RayDistance = RAY_DISTANCE;
		m_LayerMask = LayerMask.GetMask (Constants.PLAYER_STRING);
	}

	public override void Combat(GameObject target)
	{
		if (target == null)
			return;

		Vector3 targetsCenter = target.transform.FindChild (Constants.PLAYER_CENTRE_POINT).position;
		//Set our ray's origin and direction
		m_RayOrigin = transform.position;
		m_RayDirection = targetsCenter - transform.position;

		//Check our timer
		if(m_AttackTimer > ATTACK_DELAY)
		{
			//if raycast is true then call onhit for the player
			if(Raycast())
			{
				Attackable attackable = target.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject
			
				attackable.onHit(m_EnemyProjectile);
				m_AttackTimer = 0.0f;
			}
		}
		else
		{
			//Increment timer if less than delay
			m_AttackTimer += Time.deltaTime;
		}
	}

	private bool Raycast()
	{
		//Draw line for testing 
		Debug.DrawRay (m_RayOrigin, m_RayDirection);
		Debug.DrawLine (m_RayOrigin, m_RayOrigin + (m_RayDirection * m_RayDistance));

		//Check Raycast and return based on results
		if(Physics.Raycast(m_RayOrigin, m_RayDirection, m_RayDistance, m_LayerMask))
			return true;

		return false;

	}
}
