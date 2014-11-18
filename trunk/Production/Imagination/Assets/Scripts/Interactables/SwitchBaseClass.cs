using UnityEngine;
using System.Collections;
/// <summary>
/// Created by Zach Dubuc
/// 
/// A base class for Switch and PressurePlate to Inherit from
/// 
/// 
/// ChangeLog: 12/11/14 EDIT: added m_BeenHit and beenHit()
/// </summary>
public class SwitchBaseClass : MonoBehaviour
{
    protected bool m_Activated;
	protected bool m_BeenHit;

    public virtual bool getActive() //Returns whether or not the object is active
    {
        return m_Activated;
    }

	public virtual bool beenHit()//Return whether or not the switch has been hit
	{
		return m_BeenHit;
	}
}
