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
	//Override base start
    protected override void start()
    {
		//Call component start as long as it isn't null
        if (m_DeathComponent != null)
        {
            m_DeathComponent.start(this);
        }
    }

	public override void update()
	{
		
	}
}
