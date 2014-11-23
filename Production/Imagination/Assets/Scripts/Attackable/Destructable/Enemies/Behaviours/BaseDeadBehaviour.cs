/*
 * Created by Joe Burchill & Mathieu Elias November 14/2014
 * The Base Dead Behaviour which every other death behaviour will
 * inherit from. Contains Component variables and abstract update
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public abstract class BaseDeadBehaviour : BaseBehaviour 
{
    protected BaseDeath m_DeathComponent;

    public abstract void update();
}
