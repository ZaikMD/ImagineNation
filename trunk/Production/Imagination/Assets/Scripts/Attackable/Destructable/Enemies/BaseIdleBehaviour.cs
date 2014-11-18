/*
 * Created by Joe Burchill & Mathieu Elias November 14/2014
 * The Base Idle Behaviour which every other idle behaviour will
 * inherit from. Contains Component variables and abstract update
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public abstract class BaseIdleBehaviour : MonoBehaviour 
{
    protected BaseMovement m_MovementComponent;
    protected BaseEnterCombat m_EnterCombatComponent;

    public abstract void update(); 
}
