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
	public BaseMovement m_Movement;
	public BaseEnterCombat m_EnterCombat;

	// Use this for initialization
	void Start () 
	{
		m_Movement.start (this);
		m_EnterCombat.start (this);
	}
	
	// Update is called once per frame 
	public override void update()
	{
		if (m_Movement != null)
			m_Movement.Movement ();

		if (m_EnterCombat.EnterCombat())
			m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);		

	}
}
