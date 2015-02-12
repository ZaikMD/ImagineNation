using UnityEngine;
using System.Collections;

public class ZoeWeapon : BaseWeapon 
{

	int m_AttackSpeed = 8;
	int m_AttackRange = 1;

	// Use this for initialization
	void Start () 
	{
		start ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		update ();
	}

	public override void LightAttackBegin ()
	{
		m_InitialProjectilePosition = transform.position;
		m_InitialProjectileRotation = transform.rotation.eulerAngles;
		
		GameObject proj =  (GameObject)GameObject.Instantiate (m_LightColliderPrefab,
		                                                       new Vector3(m_InitialProjectilePosition.x,
		           											   m_InitialProjectilePosition.y + m_FirePointOffset,
		            										   m_InitialProjectilePosition.z), Quaternion.Euler(m_InitialProjectileRotation));
		
		LightCollider collider = proj.GetComponent<LightCollider> ();
		collider.LaunchProjectile(m_AttackSpeed,m_AttackRange);
		collider.SetCharacter (m_ReadInput.ReadInputFrom);
	}
	
	public override void LightAttackEnd ()
	{
		//Do nothing
	}
	
	public override void HeavyAttackBegin ()
	{
		m_InitialProjectilePosition = transform.position;
		m_InitialProjectileRotation = transform.rotation.eulerAngles;
		
		GameObject proj =  (GameObject)GameObject.Instantiate (m_HeavyColliderPrefab,
		                                                       new Vector3(m_InitialProjectilePosition.x,
		          											   m_InitialProjectilePosition.y + m_FirePointOffset,
		          											   m_InitialProjectilePosition.z), Quaternion.Euler(m_InitialProjectileRotation));
		
		HeavyCollider collider = proj.GetComponent<HeavyCollider> ();
		collider.LaunchProjectile(m_AttackSpeed,m_AttackRange);
		collider.SetCharacter (m_ReadInput.ReadInputFrom);
	}
	
	public override void HeavyAttackEnd ()
	{
		//Do nothing
	}
	

}
