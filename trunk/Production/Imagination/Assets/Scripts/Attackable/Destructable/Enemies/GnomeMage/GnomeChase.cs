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
	void Update () 
	{
		if (m_LeaveCombat.LeaveCombat())
			m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);

		m_Combat.Combat ();

		float dist = Vector3.Distance (transform.position, m_Targeting.CurrentTarget ()); 
		if (dist <= Constants.MAGE_ATTACK_RANGE)
			m_EnemyAI.SetState(EnemyAI.EnemyState.Attack);

	}
}
