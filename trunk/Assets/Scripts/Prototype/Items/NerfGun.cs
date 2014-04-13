using UnityEngine;
using System.Collections;

//Last updated 04/06/2014

public class NerfGun : BasePrimaryItem
{

	public GameObject m_Alex;
	public GameObject m_NerfGun;
	public GameObject m_FirePoint;

	//Current number of bullets in the clip
	int m_NumberOfBullets;

	//Maximum number of bullets in a clip
	const int maxBullets = 10;

	//Reload timer that begins after clip is emptied
	float m_ReloadTimer = 0.0f;

	//Number of seconds until the action of reloading is complete
	const float reloadTime = 3.0f;

	//Prefab that will be instantiated
	public Transform m_BulletPrefab;

	void Start () 
	{
		//Full ammo
		m_ProjectileRange = (Range)m_BulletPrefab.gameObject.GetComponent (typeof(Range));
		m_NumberOfBullets = maxBullets;
		m_NerfGun.SetActive (false);
	}
	
	void Update () 
	{
		//If the clip is empty
		if(m_NumberOfBullets <= 0)
		{

			//If the reload timer is finished
			//reload the bullets and reset the timer
			if(m_ReloadTimer >= reloadTime)
			{
				m_NumberOfBullets = maxBullets;
				m_ReloadTimer = 0.0f;
			}
			else // else continue reloading
			{
				m_ReloadTimer += Time.deltaTime;
			}
		}
	}

	public override void fire()
	{
		//As long as the clip isn't empty
		if(m_NumberOfBullets > 0)                                
		{
			m_NerfGun.SetActive(true);
			Transform tempbullet;
			//Play animation/sounds
			SoundManager.Instance.playSound(Sounds.NerfGunBullet, this.transform.position);

			tempbullet = (Transform) Instantiate(m_BulletPrefab,
			                                     m_FirePoint.transform.position,
			                                     Quaternion.identity);

			tempbullet.transform.rotation = transform.rotation;
			tempbullet.rigidbody.AddForce(getTargetDirection() * 1000);

			m_NumberOfBullets--;
		}

		else
		{
			m_NerfGun.SetActive(false);
		}
	}
		
	public override void aimFire()
	{
		//As long as the clip isn't empty call the fire function 
		//with the new target
		if(m_NumberOfBullets > 0)
		{
			m_NerfGun.SetActive(true);
			fire();
		}

		else
		{
			m_NerfGun.SetActive(false);
		}
	}

	public override float getRange()
	{
		return m_ProjectileRange.getRange ();
	}
}
