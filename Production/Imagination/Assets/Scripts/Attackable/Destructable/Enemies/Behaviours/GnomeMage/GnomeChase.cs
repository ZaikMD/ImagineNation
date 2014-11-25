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

public class GnomeChase : BaseChaseBehaviour 
{
	GameObject m_TargetPlayer;

	BaseCombat m_CombatComponent;

	// Use this for initialization
	void Start () 
	{
		m_CombatComponent.start (this);
		m_LeavingCombatComponent.start (this);
		m_MovementComponent.start (this);
		m_TargetingComponent.start (this);
	}
	
	// Update is called once per frame
	public override void update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}

		m_TargetPlayer = Target ();

		if (m_TargetPlayer == null)
		{
			m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
			return;
		}

		if (LeaveCombat())
		{
			m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
			return; 
		}

		float dist = Vector3.Distance (transform.position, m_TargetPlayer.transform.position); 
		if (dist <= Constants.MAGE_ATTACK_RANGE)
		{
			m_EnemyAI.SetState(EnemyAI.EnemyState.Attack);
			return;
		}

		Movement ();

		if (m_EnemyAI.m_UCombat)
			m_CombatComponent.Combat ();
	}
}
