using UnityEngine;
using System.Collections;
/// <summary>
/// Created by Zach Dubuc
/// 
/// A base class for Switch and PressurePlate to Inherit from
/// </summary>
public class SwitchBaseClass : MonoBehaviour
{
    bool m_Activated;

    public virtual bool getActive() //Returns whether or not the object is active
    {
        return m_Activated;
    }
}
