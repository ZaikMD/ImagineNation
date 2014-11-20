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

public class GnomeCombat : BaseBehaviour 
{
	BaseTargeting m_Targeting;
	BaseCombat m_Combat;
	BaseMovement m_Movement;

	// Use this for initialization
	void Start () 
	{
		m_Targeting.start (this);
		m_Combat.start (this);
		m_Movement.start (this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float dist = Vector3.Distance (transform.position, m_Targeting.CurrentTarget ()); 
		if (dist >= Constants.MAGE_ATTACK_RANGE)
			m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);




		m_Combat.Combat ();


	}
}
