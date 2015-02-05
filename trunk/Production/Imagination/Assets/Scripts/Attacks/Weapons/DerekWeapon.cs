using UnityEngine;
using System.Collections;

public class DerekWeapon : BaseWeapon 
{

	public Collider[] m_GloveColliders;

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
		for (int i = 0; i < m_GloveColliders.Length; i++)
		{
			m_GloveColliders[i].enabled = true;
		}
	}
	
	public override void LightAttackEnd ()
	{
		for (int i = 0; i < m_GloveColliders.Length; i++)
		{
			m_GloveColliders[i].enabled = false;
		}
	}
	
	public override void HeavyAttackBegin ()
	{
		throw new System.NotImplementedException ();
	}
	
	public override void HeavyAttackEnd ()
	{
		throw new System.NotImplementedException ();
	}
	
	public override void ConeAttack ()
	{
		throw new System.NotImplementedException ();
	}
	
	public override void AOEAttack ()
	{
		throw new System.NotImplementedException ();
	}
}
