/*

3/29/2014 - Jason "The Casual" Hein
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
		m_ProjectileRange = (Range) m_DerekProjectile.GetComponent (typeof(Range));


		//m_Derek = getDerek ();
		m_BaseProjectile = m_DerekProjectile;
	}
	
	void Update()
	{
	}
	
	public override void fire()
	{
		 SoundManager.Instance.playSound(Sounds.BoxingGloveImpact, this.transform.position);
		 GameObject projectile = (GameObject)Instantiate (m_BaseProjectile, this.transform.position, this.transform.rotation);

		projectile.gameObject.GetComponent<DerekProjectile> ().setForwardDirection(getTargetDirection());
		//getTargetDirection();	//Use for getting firing direction
	}



	public override float getRange()
	{
		return m_ProjectileRange.getRange ();
	}
}
