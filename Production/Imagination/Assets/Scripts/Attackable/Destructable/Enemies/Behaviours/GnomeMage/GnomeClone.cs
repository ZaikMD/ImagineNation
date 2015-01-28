/*
 * Created by Mathieu Elias
 * Date: Nov 24, 2014
 *  
 * This class is the clones which the gnome mage creates
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion
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

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// The function to call when creating a clone to initialise everything
	public void Create(BaseMovement moveComponent, BaseCombat combatComponent, Vector3 startingPos, 
	                   float activeTime, GameObject target, GameObject projectilePrefab)
	{
		// Initialise the target
		m_Target = target;
		// Add a navmesh agent 
		m_Agent = (NavMeshAgent) gameObject.AddComponent (Constants.NAV_AGENT);

		// Create a move component of the same type as the one passed
		System.Type movementType = moveComponent.GetType ();
		string moveType = movementType.ToString ();
		m_MovementComponent = (BaseMovement) gameObject.AddComponent (moveType);

		// Set the agent of that move component
		if (m_MovementComponent != null)
			m_MovementComponent.SetAgent (m_Agent);

		//Create a combat component of the same type as the one passed
		System.Type combatType = combatComponent.GetType ();
		string comType = combatType.ToString ();
		m_CombatComponent = (BaseCombat) gameObject.AddComponent (comType);

		// Initialise the projectile prefab
		if (m_CombatComponent != null)
			m_CombatComponent.SetProjectilePrefab (projectilePrefab);

		// Set the active timer
		m_ActiveTimer = activeTime;

		// move to the starting attack position
		if (m_MovementComponent != null)
			m_MovementComponent.Movement (startingPos);

		m_Health = 0.5f;
	}
	
	// Update is called once per frame
	new void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }
	
		// If the clones active timer is done or if the original is dead then kill it off
		if (m_ActiveTimer <= 0.0f || m_OriginalIsDead)
		{ 
			instantKill();
		}

		// If the target is not null look at it 
		if (m_Target != null)
			transform.LookAt (m_Target.transform.position);

		//Attack if the shot timer is over
		if (m_ShotTimer <= 0)
		{
			if (m_CombatComponent != null)
				m_CombatComponent.Combat (m_Target);

			m_ShotTimer = m_TimeBetweenShots;
		}

		// Move
		if (m_MovementComponent != null)
			m_MovementComponent.Movement (m_Target);

		m_ActiveTimer -= Time.deltaTime;
	}
}
