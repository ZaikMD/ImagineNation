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

	public void Create(BaseMovement moveComponent, BaseCombat combatComponent, Vector3 startingPos, float activeTime, GameObject target)
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

		m_ActiveTimer = activeTime;

		m_MovementComponent.Movement (startingPos);
	}
	
	// Update is called once per frame
	void Update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}
	
		if (m_ActiveTimer <= 0.0f || m_OriginalIsDead)
		{
			instantKill();
		}

		m_CombatComponent.Combat ();
		m_MovementComponent.Movement (m_Target);

		m_ActiveTimer -= Time.deltaTime;
	}
}
