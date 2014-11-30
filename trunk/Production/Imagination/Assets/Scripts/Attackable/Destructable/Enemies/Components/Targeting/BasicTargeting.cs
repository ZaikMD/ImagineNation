/*
 * Created by Mathieu Elias November 24/2014
 * 
 * This basic targeting script will just directly return the player with the highest threat 
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class BasicTargeting : BaseTargeting 
{
	public override GameObject CurrentTarget ()
	{
		// if the perception script is not null we return the player with the highest threat. 
		if (m_Perception != null)
			return m_Perception.getHighestThreatPlayer().gameObject;		

		//else we return null
		return null;
	}

}
