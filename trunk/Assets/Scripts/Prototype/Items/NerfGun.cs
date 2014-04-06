using UnityEngine;
using System.Collections;

public class NerfGun : BasePrimaryItem
{

	public GameObject m_Alex;

	//Current number of bullets in the clip
	int m_NumberOfBullets;

	//Maximum number of bullets in a clip
	const int maxBullets = 5;

	//Reload timer that begins after clip is emptied
	float m_ReloadTimer = 0.0f;

	//Number of seconds until the action of reloading is complete
	const float reloadTime = 3.0f;

	//Prefab that will be instantiated
	public Transform m_BulletPrefab;

	void Start () 
	{
		//Full ammo
		m_NumberOfBullets = maxBullets;
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

	/// <summary>
	/// An overrided fire function from the BasePrimaryItem class
	/// Is responsible for instantiating bullet and moving it
	/// </summary>
	public override void fire()
	{
		//As long as the clip isn't empty
		if(m_NumberOfBullets > 0)                                
		{

			Transform tempbullet;
			//Play animation/sounds

			tempbullet = (Transform) Instantiate(m_BulletPrefab,
			                                     transform.position,
			                                     Quaternion.identity);

			tempbullet.transform.rotation = transform.rotation;
			tempbullet.rigidbody.AddForce(getTargetDirection() * 1000);

			m_NumberOfBullets--;
		}
	}

	/// <summary>
	/// An overrided aimFire function from the BasePrimaryItem class
	/// Responsible for calling the fire function but with a new target based on the reticle
	/// </summary>
	public override void aimFire()
	{
		//As long as the clip isn't empty call the fire function 
		//with the new target
		if(m_NumberOfBullets > 0)
		{
			fire();
		}
	}
}
