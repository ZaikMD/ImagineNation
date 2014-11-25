using UnityEngine;
using System.Collections;
/*
 * Created by Mathieu Elias
 * Date: Nov 14, 2014
 *  
 * This script handles the functionality of the Gnome Mage enemy
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion

public class GnomeCombat : BaseAttackBehaviour 
{
	enum CombatStates
	{
		Regular = 0,
		Cloning,
		Cloned
	}
	CombatStates m_CurrentCombatState = CombatStates.Regular;

	BaseMovement m_CloningMovement;

	BaseCombat m_ClonedCombat;
	BaseMovement m_ClonedMovement;

	public const float m_ClonedTime = 3.0f;
	float m_ClonedTimer = 0.0f;

	GnomeShield m_Shield;

	// Use this for initialization
	void Start () 
	{
		m_TargetingComponent.start (this);

		m_CombatComponent.start (this);
		m_MovementComponent.start (this);

		m_ClonedCombat.start (this);
		m_ClonedMovement.start (this);

		m_Shield = GetComponentInParent<GnomeShield> ();
	}
	
	// Update is called once per frame
	public override void update ()
	{
		m_Target = Target ();

		if (m_Target == null)
		{
			m_EnemyAI.SetState (EnemyAI.EnemyState.Idle);
			return;
		}

		float dist = Vector3.Distance (transform.position, m_Target.transform.position); 
		if (dist >= Constants.MAGE_ATTACK_RANGE)
			m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);

		switch (m_CurrentCombatState)
		{
		case CombatStates.Regular:
			Regular();
			break;

		case CombatStates.Cloning:
			Cloning();
			break;

		case CombatStates.Cloned:
			Cloned();
			break;
		}
	}

	void Regular()
	{
		Movement ();
		Combat ();

		if (m_EnemyAI.m_Health == 1)
		{
			DeactivateShield();
			m_CurrentCombatState = CombatStates.Cloning;
		}
	}

	void Cloning()
	{
		Movement ();
		CreateClones ();
	}

	void Cloned()
	{
		if (m_ClonedTimer <= 0)
			m_CurrentCombatState = CombatStates.Regular;		

		Movement ();
		Combat ();
		m_ClonedTimer -= Time.deltaTime;
	}

	void CreateClones()
	{
		m_ClonedTimer = m_ClonedTime;
		m_CurrentCombatState = CombatStates.Cloned;
	}

	void DeactivateShield()
	{
		m_Shield.DeactivateShield (m_ClonedTime);
	}

	protected override void Movement ()
	{
		if (m_EnemyAI.m_UMovement)
		{
			switch (m_CurrentCombatState)
			{
			case CombatStates.Regular:
				if (m_MovementComponent != null)			
					m_MovementComponent.Movement();
				break;
				
			case CombatStates.Cloning:
				if (m_CloningMovement != null)	
					m_CloningMovement.Movement();
				break;
				
			case CombatStates.Cloned:
				if (m_ClonedMovement != null)	
					m_ClonedMovement.Movement();				
				break;
			}
		}
	}

	protected override void Combat ()
	{
		if (m_EnemyAI.m_UCombat)
		{
			switch (m_CurrentCombatState)
			{
			case CombatStates.Regular:
				if (m_CombatComponent != null)
					m_CombatComponent.Combat();				
				break;
				
			case CombatStates.Cloned:
				if (m_ClonedCombat != null)
					m_ClonedCombat.Combat();		
				break;
			}
		}
	}
}
