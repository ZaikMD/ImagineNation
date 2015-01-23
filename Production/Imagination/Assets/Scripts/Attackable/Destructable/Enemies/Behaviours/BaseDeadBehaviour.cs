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

		names [0] = Constants.DEATH_STRING;
		components[0] = m_DeathComponent;
	}

	public override string BehaviourType()
	{
		return Constants.DEAD_BEHAVIOUR_STRING;
	}

	public override int numbComponents ()
	{
		return 1;
	}
	
	public override void SetComponents (string[] components)
	{
		m_ComponentsObject = transform.FindChild (Constants.COMPONENTS_STRING).gameObject;
		
		m_DeathComponent = m_ComponentsObject.GetComponent (components [0]) as BaseDeath;

	}
}
