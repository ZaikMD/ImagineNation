using UnityEngine;
using System.Collections;

public class DerekProjectile : MonoBehaviour 
{
	public GameObject m_DebrisPrefab;
	public float m_Speed = 10.0f;
	public float m_ProjectileRange = 2.0f;
	Vector3 m_InitialPosition;

	Vector3 m_Direction;
	// Use this for initialization
	void Start () 
	{
		m_InitialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += m_Direction * m_Speed * Time.deltaTime;
		float distance = Vector3.Distance (m_InitialPosition, transform.position);
		if(distance > m_ProjectileRange)
		{
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			Destructable enemy = (Destructable)other.gameObject.GetComponent(typeof(Destructable));
			
			enemy.applyDamage(10);
			Destroy(this.gameObject);
		}

		if(other.gameObject.tag == "DestructibleWall")
		{
			//play sound
			SoundManager.Instance.playSound(Sounds.BreakingObject, this.transform.position);
			Destroy(other.gameObject);

			Instantiate(m_DebrisPrefab, other.transform.position, other.transform.rotation);

			Destroy(this.gameObject);

			//StartCoroutine (DestroyDebris ());
	
		}
	}

	public float getProjectileRange()
	{
		return m_ProjectileRange;
	}


	public void setForwardDirection(Vector3 direction)
	{
		m_Direction = direction;
	}
//	IEnumerator DestroyDebris()
//	{
//		yield return new WaitForSeconds (2.0f);
//		Destroy (m_DebrisPrefab.gameObject);
//	}

}
