using UnityEngine;
using System.Collections;

public class CollisionCombat : BaseCombat 
{
	public EnemyProjectile m_EnemyProjectile;

	private Vector3 m_RayOrigin;
	private Vector3 m_RayDirection;
	private float m_RayDistance;
	private int m_LayerMask;
	private const float RAY_DISTANCE = 0.05f;

	private GameObject m_PlayerCenterPoint;
	private GameObject m_Player;

	private float m_AttackTimer;
	private const float ATTACK_DELAY = 1.5f;


	public override void start (BaseBehaviour baseBehaviour)
	{
		base.start (baseBehaviour);

		m_PlayerCenterPoint = GameObject.Find (Constants.PLAYER_CENTRE_POINT);
		m_Player = GameObject.FindGameObjectWithTag (Constants.PLAYER_STRING);
		m_RayDistance = RAY_DISTANCE;
		m_LayerMask = ~LayerMask.GetMask (Constants.PLAYER_STRING);
	}

	public override void Combat()
	{
		m_RayOrigin = transform.position;
		m_RayDirection = m_PlayerCenterPoint.transform.position - transform.position;

		if(m_AttackTimer > ATTACK_DELAY)
		{
			if(Raycast())
			{
				Attackable attackable = m_Player.gameObject.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject
			
				attackable.onHit(m_EnemyProjectile);
				m_AttackTimer = 0.0f;
			}
		}
		else
		{
			m_AttackTimer += Time.deltaTime;
		}
	}

	private bool Raycast()
	{
		if(Physics.Raycast(m_RayOrigin, m_RayDirection, m_RayDistance, m_LayerMask))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
