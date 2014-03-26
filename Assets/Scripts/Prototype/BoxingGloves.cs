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
		fire ();
	}
	
	public void fire()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Instantiate (m_BaseProjectile, m_BaseProjectile.transform.position, transform.rotation);
		}
	}

	public void aimFire()
	{

	}

}
