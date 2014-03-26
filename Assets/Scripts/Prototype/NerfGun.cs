using UnityEngine;
using System.Collections;

public class NerfGun : MonoBehaviour
{

	public GameObject m_Alex;

	//Current number of bullets in the clip
	int m_NumberOfBullets;

	//Maximum number of bullets in a clip
	const int maxBullets = 5;

	int m_BulletsUsed = -1;

	//Reload timer that begins after clip is emptied
	float m_ReloadTimer = 0.0f;

	//Number of seconds until the action of reloading is complete
	const float reloadTime = 3.0f;

	//Pool of bullets
	Transform[] BulletPool;

	//Number of bullets in pool
	int bulletPool = 10;

	//Prefab that will be instantiated
	public Transform m_BulletPrefab;

	//List<Transform> m_BulletPool = new List<Transform>();

	//Create a public camera variable
	
	void Start () 
	{
		//Full ammo
		m_NumberOfBullets = maxBullets;

		//Instantiate the bullet pool
		BulletPool = new Transform[bulletPool];

		//Instantiate the bullet prefab, add them to the bullet pool, and set them to inactive
		for(int i = 0; i < bulletPool; i++)
		{

			BulletPool[i] = (Transform) Instantiate(m_BulletPrefab,
			                            			transform.position,
			                            			Quaternion.identity);

			//BulletPool[i].gameObject.GetComponent<NerfGunProjectile>().setActive(false);
		}



	}
	
	void Update () 
	{
		//
		if(m_NumberOfBullets <= 0)
		{
			if(m_ReloadTimer >= reloadTime)
			{
				m_NumberOfBullets = maxBullets;
				m_ReloadTimer = 0.0f;
			}
			else
			{
				m_ReloadTimer += Time.deltaTime;
			}
		}

		//Temporary code to test the fire function
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Fire(new Vector3(0,30,0));
		}
	}

	public void Fire(Vector3 currentTarget)
	{
		//As long as the clip isn't empty
		if(m_NumberOfBullets > 0)                                
		{

			Transform tempbullet;
			//Play animation/sounds
			for(int i = 0; i < bulletPool; i++)
			{
				GameObject temp = BulletPool[i].gameObject;
				NerfGunProjectile tempProj = temp.GetComponent<NerfGunProjectile>();
				if(tempProj.getActive() == false)
				{
					tempbullet = BulletPool[i];
					BulletPool[i].gameObject.GetComponent<NerfGunProjectile>().setActive(true);

					tempbullet.transform.rotation = transform.rotation;
					tempbullet.rigidbody.AddForce(currentTarget * 5000);
					
					m_BulletsUsed++;
					m_NumberOfBullets--;
					break;
				}
			}
		}
	}
		
	public void aimFire(Vector3 currentTarget)
	{
		//As long as the clip isn't empty call the fire function 
		//with the new target
		if(m_NumberOfBullets > 0)
		{
			Fire(currentTarget);
		}
	}
}
