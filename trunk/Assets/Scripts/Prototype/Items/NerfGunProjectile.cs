using UnityEngine;
using System.Collections;

enum NerfGunProjectileState
{
	Default = 0,
	IsMoving,
	IsPlatform,
	Count,
	Unknown
};

public class NerfGunProjectile : MonoBehaviour 
{

	NerfGunProjectileState m_State;

	float m_BulletLifeSpan = 5.0f;
	float m_PlatformLifeSpan = 20.0f;
	float m_Timer = 0.0f;
	float m_PlatformTimer = 0.0f;
	float m_Speed = 15.0f;

	string m_CollidedTag;

	bool m_IsPlatform = false;

	bool m_Active;

	// Use this for initialization
	void Start () 
	{
		m_State = NerfGunProjectileState.Default;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_State == NerfGunProjectileState.Default)
		{
			DefaultState();
		}

		switch(m_State)
		{
		case NerfGunProjectileState.IsPlatform :
			IsPlatformState();
			break;

		case NerfGunProjectileState.IsMoving :
			IsMovingState();
			break;
		}
	}

	void DefaultState()
	{
		//the Default state simply determines which of the two other states
		//the projectile is in or should be in.
		if(m_IsPlatform == true)
		{
			m_State = NerfGunProjectileState.IsPlatform;
		}
		else
		{
			m_State = NerfGunProjectileState.IsMoving;
		}
	}

	void IsPlatformState()
	{
		//update the platform timer and if the timer has expired delete the platform
		//potential for adding a growing and shrinking platform proportionally based
		// on the time remaing.

		m_PlatformTimer += Time.deltaTime;


		//transform.localScale = new Vector3 (transform.localScale.x + 1.0f, 0.1f, 1.5f);

		if(m_PlatformTimer >= m_PlatformLifeSpan)
			Destroy (this.gameObject);
	}
	
	void IsMovingState()
	{
		//update the bullets general timer and destroy it if it exceeds its set lifespan
		m_Timer += Time.deltaTime;

		if (m_Timer >= m_BulletLifeSpan)
		{
			Destroy (this.gameObject);
		}
	}
	
	void OnCollisionEnter(Collision other)
	{             
		//Essentially this switch statement checks the tag of the object the bullet collided
		// with and enters the corresponding case, if none of the 3 possible options were
		// collided with the bullet by default is destroyed

		m_CollidedTag = other.gameObject.tag;

		switch(m_CollidedTag)
		{
			//All cases call a function that takes the gameobject of the object
			//that the bullet collided with
		case "Enemy" :
				CollidedWithEnemy(other.gameObject);
			break;
			
		case "NerfWall" :
				CollidedWithNerfWall(other.gameObject);
			break;
			
		case "NerfTarget" :
				CollidedWithNerfTarget(other.gameObject);
			break;

		case "Player" :
		{
			if(m_State == NerfGunProjectileState.IsMoving)
			{
				Destroy (this.gameObject);
				break;
			}
			else
			{
				if(other.gameObject.name == "Alex")
				{
					Destroy (this.gameObject);
				}
			}
			break;
		}
			
		default:
			Destroy (this.gameObject);
			break;
		}
	}
	
	void CollidedWithEnemy(GameObject enemy)
	{
		Destroy (this.gameObject);              
		//enemy.applyDamage();
	}
	
	void CollidedWithNerfWall(GameObject nerfWall)
	{
		//freeze the bullets position at the point of collision       
		//preferably increase the size of the bullet or
		// destroy the bullet and spawn a platform
		transform.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		transform.localScale = new Vector3 (transform.localScale.x + 1.0f, 0.1f, 1.5f);
		m_IsPlatform = true;
		m_State = NerfGunProjectileState.IsPlatform;

	}
	
	void CollidedWithNerfTarget(GameObject nerfTarget)
	{             
		//activate trigger is simply a placeholder name for a function within
		//a nerf target script that will perform the intended response upon
		//being hit
		Destroy (this.gameObject);               
		nerfTarget.SetActive (false);
	}

	public void setActive(bool active)
	{
		m_Active = active;
		//this.gameObject.SetActive (m_Active);
		Debug.Log (m_Active);
	}

	public bool getActive()
	{
		return m_Active;
	}
}
