/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
 * The base class for all targeting components.
 * The Targeting components use the information from the perception script 
 * and decide which of the players is the current target.
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public abstract class BaseTargeting : BaseComponent
{
	protected Perception m_Perception;

    public override void start(BaseBehaviour baseBehaviour)
    {
		m_Perception = GetComponentInParent<Perception> ();
    }

	// All targeting components must have this function,
	// it is meant to decide the current target then return it.
    public abstract GameObject CurrentTarget();
}
