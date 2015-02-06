using UnityEngine;
using System.Collections;

public class HeavyCollider : MonoBehaviour 
{
	public const float KNOCKBACK = 1.2f;
	
	public float m_Damage = 1.0f;
	
	void OnTriggerEnter( Collider obj)
	{
		if (obj.gameObject.GetComponent(typeof(Attackable)) as Attackable != null)//checks to see if the object that has been hit is attackable
		{
			Attackable attackable = obj.gameObject.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject
			
			attackable.onHit(this, m_Damage);
		} 
	}
}
