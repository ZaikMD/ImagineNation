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

	BaseTargeting m_Targeting;
	GameObject m_Target;

	BaseCombat m_RegularCombat;
	BaseMovement m_RegularMovement;

	BaseMovement m_CloningMovement;

	BaseCombat m_ClonedCombat;
	BaseMovement m_ClonedMovement;

	public const float m_ClonedTime = 3.0f;
	float m_ClonedTimer = 0.0f;

	GnomeShield m_Shield;

	// Use this for initialization
	void Start () 
	{
		m_Targeting.start (this);

		m_RegularCombat.start (this);
		m_RegularMovement.start (this);

		m_ClonedCombat.start (this);
		m_ClonedMovement.start (this);

		m_Shield = GetComponentInParent<GnomeShield> ();
	}
	
	// Update is called once per frame
	public override void update ()
	{
		m_Target = m_Targeting.CurrentTarget ();

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
		if (m_RegularMovement != null)
		m_RegularMovement.Movement ();

		m_ClonedCombat.Combat ();

		if (m_EnemyAI.m_Health == 1)
		{
			DeactivateShield();
			m_CurrentCombatState = CombatStates.Cloning;
		}
	}

	void Cloning()
	{
		if (m_CloningMovement != null)
		m_CloningMovement.Movement ();

		CreateClones ();
	}

	void Cloned()
	{
		if (m_ClonedTimer <= 0)
			m_CurrentCombatState = CombatStates.Regular;		

		if (m_ClonedMovement != null)
			m_ClonedMovement.Movement ();

		m_ClonedCombat.Combat ();
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
}
