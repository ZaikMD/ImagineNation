﻿/*
 * Created by Joe Burchill November 14/2014
 * Dead Behaviour for the Furbull, calls
 * Dead Component
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class FurbullDeadBehaviour : BaseDeadBehaviour 
{
    void Start()
    {
        m_DeathComponent.start(this);
    }

	public override void update()
	{
		//Add in Death Functionality
	}
}