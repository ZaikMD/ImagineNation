using UnityEngine;
using System.Collections;

public class DerekWeapon : BaseWeapon 
{

	 LightCollider[] m_LightCollider;
	 HeavyCollider[] m_HeavyCollider;

	// Use this for initialization
	void Start () 
	{
		start ();
		m_LightCollider = transform.GetComponentsInChildren<LightCollider> ();
		m_HeavyCollider = transform.GetComponentsInChildren<HeavyCollider> ();

		for (int i = 0; i < m_LightCollider.Length; i++)
		{
			m_LightCollider[i].Activate(false);
			m_HeavyCollider[i].Activate(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		update ();
	}

	public override void LightAttackBegin ()
	{
		for (int i = 0; i < m_LightCollider.Length; i++)
		{
			m_LightCollider[i].Activate(true);
		}
	}
	
	public override void LightAttackEnd ()
	{
		for (int i = 0; i < m_LightCollider.Length; i++)
		{
			m_LightCollider[i].Activate(false);
		}
	}
	
	public override void HeavyAttackBegin ()
	{
		for (int i = 0; i < m_HeavyCollider.Length; i++)
		{
			m_HeavyCollider[i].Activate(true);
		}
	}
	
	public override void HeavyAttackEnd ()
	{
		for (int i = 0; i < m_HeavyCollider.Length; i++)
		{
			m_HeavyCollider[i].Activate(false);
		}
	}
}
