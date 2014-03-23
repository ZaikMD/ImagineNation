using UnityEngine;
using System.Collections;

public class NerfGun : MonoBehaviour 
{

	public GameObject m_Alex;

	int m_NumberOfBullets;
	const int maxBullets = 5;

	int m_BulletsUsed = -1;

	float m_ReloadTimer = 0.0f;
	const float reloadTime = 3.0f;

	GameObject[] BulletPool;
	public Transform m_BulletPrefab;

	//Create a public camera variable
	
	void Start () 
	{
		//Full ammo
		m_NumberOfBullets = maxBullets;
	}

	void Update () 
	{
		if(m_NumberOfBullets >= maxBullets)
		{
			if(m_ReloadTimer >= reloadTime)
			{
				m_NumberOfBullets = maxBullets;
				m_ReloadTimer = 0.0f;
			}

			m_ReloadTimer += Time.deltaTime;
		}
	}

	public void Fire(Vector3 currentTarget)
	{
		if(m_NumberOfBullets > 0)                                
		{
			//Play animation/sounds
			//grab a bullet from the pool of bullets
			int i = m_BulletsUsed + 1;
			GameObject tempBullet = BulletPool[i];
			/*
			BulletPool[i].transform = Instantiate(m_BulletPrefab, 
			                                      transform.position, 
			                                      Quaternion.identity);

			BulletPool[i].transform.rotation = transform.rotation;
			BulletPool[i].rigidbody.AddForce(currentTarget * 5000);

			m_BulletsUsed++;
			m_NumberOfBullets--;
			*/
		}
	}
		
	public void aimFire(Vector3 currentTarget)
	{
		//Use the camera variable to get the reticle position
		//Set that position to the currentTarget
		if(m_NumberOfBullets > 0)
		{
			Fire(currentTarget);
		}
	}
}
