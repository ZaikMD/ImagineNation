using UnityEngine;
using System.Collections;

public class BoxingGloves : BasePrimaryItem 
{
	GameObject m_Derek;
	GameObject m_DerekProjectile;
	bool m_EnableProjectile = false;

	// Use this for initialization
	void Start () 
	{
		//m_Derek = getDerek ();
		//m_DerekProjectile = m_BaseProjectile;
	}
	
	public void fire(Vector3 currentTarget)
	{
		if(m_EnableProjectile == false)
		{
			Instantiate (m_DerekProjectile, m_DerekProjectile.transform.position, transform.rotation);
			m_EnableProjectile = true;
		}
	}

	public void aimFire(Vector3 currentTarget)
	{
		fire (currentTarget);
	}

}
