using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour 
{
	void OnTriggerEnter(Collider obj)
	{
		Destructable objDestructable = (Destructable)obj.GetComponentInChildren<Destructable>();
		if(objDestructable != null)
		{
			objDestructable.instantKill();
		}
	}
}
