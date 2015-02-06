using UnityEngine;
using System.Collections;

public class BaseCollider : MonoBehaviour 
{

	public float KNOCKBACK;
	
	public float m_Damage;
	
	protected bool m_IsActive = false;


	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Activate(bool isActive)
	{
		m_IsActive = isActive;
	}


}
