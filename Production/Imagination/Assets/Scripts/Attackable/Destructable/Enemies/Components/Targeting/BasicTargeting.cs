using UnityEngine;
using System.Collections;

public class BasicTargeting : BaseTargeting 
{
	public override GameObject CurrentTarget ()
	{
		if (m_Perception != null)
			return m_Perception.getHighestThreatPlayer().gameObject;
		

		return null;
	}

}
