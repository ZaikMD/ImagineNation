using UnityEngine;
using System.Collections;

public class LightCollider : BaseCollider 
{

	// Use this for initialization
	void Start () 
	{
		KNOCKBACK = 0.6f;
		
		m_Damage = 0.5f;
	}


	void OnTriggerEnter( Collider obj)
	{
		if (m_IsActive)
		{
			if (obj.gameObject.GetComponent(typeof(Attackable)) as Attackable != null)//checks to see if the object that has been hit is attackable
			{
				Attackable attackable = obj.gameObject.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject
				
				attackable.onHit(this, m_Damage);
			} 
		}
	}
}
