/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
 * Base class for all entercombat components
 * The enter combat components dictate how the enemy chooses to enter combat and returns wether he is entering or not
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public abstract class BaseEnterCombat : BaseComponent
{
    public override void start(BaseBehaviour baseBehaviour)
    {

    }

	// All enter combat components must use this function to decide if its time for the enemy to enter combat and return it
	public abstract bool EnterCombat(Transform target);
}
