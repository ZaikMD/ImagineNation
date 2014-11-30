/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
 * Base class for all death components
 * The death components are used to do anything that must be done when the enemy dies
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public abstract class BaseDeath : BaseComponent
{
    public override void start(BaseBehaviour baseBehaviour)
    {

    }

	//All death components must use this function to take care of anything that happens when the enemy dies
	public abstract void Death();
}
