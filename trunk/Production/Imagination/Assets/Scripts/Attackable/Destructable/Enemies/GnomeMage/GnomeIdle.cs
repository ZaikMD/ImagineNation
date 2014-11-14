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

public class GnomeIdle : BaseBehaviour 
{
	public BaseMovement m_Movement;
	public BaseEnterCombat m_EnterCombat;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_Movement.Movement ();

		if (m_EnterCombat.EnterCombat())
		{
			//switch state
		}
	}
}
