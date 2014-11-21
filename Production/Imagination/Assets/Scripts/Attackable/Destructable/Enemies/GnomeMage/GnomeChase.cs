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
	BaseCombat m_Combat;

	BaseTargeting m_Targeting;
	GameObject m_TargetPlayer;

	BaseLeavingCombat m_LeaveCombat;

	// Use this for initialization
	void Start () 
	{
		m_Combat.start (this);
		m_LeaveCombat.start (this);
	}
	
	// Update is called once per frame
	public override void update () 
	{
		m_TargetPlayer = m_Targeting.CurrentTarget ();

		if (m_TargetPlayer == null)
		{
			m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
			return;
		}

		if (m_LeaveCombat.LeaveCombat())
		{
			m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
			return; 
		}

		m_Combat.Combat ();

		float dist = Vector3.Distance (transform.position, m_TargetPlayer.transform.position); 
		if (dist <= Constants.MAGE_ATTACK_RANGE)
			m_EnemyAI.SetState(EnemyAI.EnemyState.Attack);

	}
}
