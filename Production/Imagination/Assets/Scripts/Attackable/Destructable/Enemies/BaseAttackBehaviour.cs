/*
 * Created by Joe Burchill & Mathieu Elias November 14/2014
 * The Base Attack Behaviour which every other attack behaviour will
 * inherit from. Contains Component variables and abstract update
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public abstract class BaseAttackBehaviour : BaseBehaviour 
{
    protected BaseCombat m_CombatComponent;
    protected BaseTargeting m_TargetingComponent;
    protected BaseMovement m_MovementComponenet;

    public abstract void update();
}
