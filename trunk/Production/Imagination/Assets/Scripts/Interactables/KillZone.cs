/*
*KillZone
*
*resposible for killing things instantly
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour 
{
	//if the object is destructable kill it
	void OnTriggerEnter(Collider obj)
	{
		Destructable objDestructable = (Destructable)obj.GetComponentInChildren<Destructable>();
		if(objDestructable != null)
		{
			objDestructable.instantKill();
		}
	}
}
