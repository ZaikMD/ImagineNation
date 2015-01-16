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
	//The component all death behaviours must have
	public BaseDeath m_DeathComponent;

    public abstract void update();

	public override void ComponentInfo (out string[] names, out BaseComponent[] components)
	{
		names = new string[1];
		components = new BaseComponent[1];

		names [0] = "Death";
		components[0] = m_DeathComponent;
	}

	public override string BehaviourType()
	{
		return "Death Behaviour";
	}
}
