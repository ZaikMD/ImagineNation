/*
 * Created by Joe Burchill November 14/2014
 * Dead Behaviour for the Spin Top, calls
 * Dead Component
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class SpinningTopDeadBehaviour : BaseDeadBehaviour 
{
    protected override void start()
    {
        if (m_DeathComponent != null)
        {
            m_DeathComponent.start(this);
        }
    }

	public override void update()
	{
		
	}
}
