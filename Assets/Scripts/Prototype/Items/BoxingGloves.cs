/*

3/29/2014 - Jason
	Now properly overrides base primary item's fire functions


*/


using UnityEngine;
using System.Collections;

public class BoxingGloves : BasePrimaryItem 
{
	public GameObject m_DerekProjectile;
	GameObject m_Derek;

	// Use this for initialization
	void Start () 
	{
		//m_Derek = getDerek ();
		m_BaseProjectile = m_DerekProjectile;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F))
		{
			fire (transform.position + transform.forward);
		}
	}
	
	public override void fire(Vector3 target)
	{
		Instantiate (m_BaseProjectile, this.transform.position, this.transform.rotation);
	}
}
