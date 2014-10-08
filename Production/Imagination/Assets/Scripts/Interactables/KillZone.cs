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
