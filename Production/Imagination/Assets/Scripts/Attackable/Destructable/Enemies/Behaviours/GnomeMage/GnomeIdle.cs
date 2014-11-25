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

public class GnomeIdle : BaseIdleBehaviour 
{
	// Use this for initialization
	protected override void start ()
	{
		m_MovementComponent.start (this);
		m_EnterCombatComponent.start (this);
		m_TargetingComponent.start (this);
	}
	
	// Update is called once per frame 
	public override void update()
	{
		m_Target = Target ();

		if (m_MovementComponent != null)
			Movement ();

		if (m_Target != null)
		{
			if (EnterCombat(m_Target.transform))
				m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
		}
	}
}
