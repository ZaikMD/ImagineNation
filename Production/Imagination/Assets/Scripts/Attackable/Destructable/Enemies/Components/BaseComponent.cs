/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * // A base class for all components. For now its only use is to make sure all components
 * have a start function which will be called by the behaviour which has the component
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public abstract class BaseComponent : MonoBehaviour 
{
	//YOU MUST CALL THIS START FOR INSIDE EVERY BEHAVIOUR THAT HAS A COMPONENT
    public abstract void start(BaseBehaviour baseBehaviour);
}
