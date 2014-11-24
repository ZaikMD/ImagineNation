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
	void Start () 
	{
		m_MovementComponent.start (this);
		m_EnterCombatComponent.start (this);
	}
	
	// Update is called once per frame 
	public override void update()
	{
		if (m_MovementComponent != null)
			Movement ();

		if (EnterCombat())
			m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);		

	}
}
