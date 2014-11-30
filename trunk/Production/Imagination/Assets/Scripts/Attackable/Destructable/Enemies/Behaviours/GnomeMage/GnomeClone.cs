using UnityEngine;
using System.Collections;

public class GnomeClone : Destructable 
{
	BaseMovement m_MovementComponent;
	BaseCombat m_CombatComponent;

	float m_ActiveTimer = 0.0f;

	NavMeshAgent m_Agent;

	GameObject m_Target;
	public bool m_OriginalIsDead = false;

	const float m_TimeBetweenShots = 1.0f;
	float m_ShotTimer = 0.0f;

	public void Create(BaseMovement moveComponent, BaseCombat combatComponent, Vector3 startingPos, 
	                   float activeTime, GameObject target, GameObject projectilePrefab)
	{
		m_Target = target;
		m_Agent = (NavMeshAgent) gameObject.AddComponent (Constants.NAV_AGENT);

		System.Type movementType = moveComponent.GetType ();
		string moveType = movementType.ToString ();
		m_MovementComponent = (BaseMovement) gameObject.AddComponent (moveType);

		m_MovementComponent.SetAgent (m_Agent);

		System.Type combatType = combatComponent.GetType ();
		string comType = combatType.ToString ();
		m_CombatComponent = (BaseCombat) gameObject.AddComponent (comType);

		m_CombatComponent.SetProjectilePrefab (projectilePrefab);

		m_ActiveTimer = activeTime;

		m_MovementComponent.Movement (startingPos);
	}
	
	// Update is called once per frame
	new void Update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}
	
		if (m_ActiveTimer <= 0.0f || m_OriginalIsDead)
		{
			instantKill();
		}

		if (m_Target != null)
			transform.LookAt (m_Target.transform.position);

		if (m_ShotTimer <= 0)
		{
			m_CombatComponent.Combat ();
			m_ShotTimer = m_TimeBetweenShots;
		}

		m_MovementComponent.Movement (m_Target);

		m_ActiveTimer -= Time.deltaTime;
	}
}
