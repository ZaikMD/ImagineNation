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
		if(Input.GetKeyDown(KeyCode.Space))
		{
			fire ();
		}
	}
	
	public void fire()
	{
		Instantiate (m_BaseProjectile, this.transform.position, this.transform.rotation);
	}

	public void aimFire()
	{

	}

}
