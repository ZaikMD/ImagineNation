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
			m_LightCollider[i].SetCharacter(m_ReadInput.ReadInputFrom);

			m_HeavyCollider[i].Activate(false);
			m_HeavyCollider[i].SetCharacter(m_ReadInput.ReadInputFrom);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		update ();
	}

	public override void AttackBegin ()
	{
		for (int i = 0; i < m_LightCollider.Length; i++)
		{
			m_LightCollider[i].Activate(true);
		}
	}
	
	public override void AttackEnd ()
	{
		for (int i = 0; i < m_LightCollider.Length; i++)
		{
			m_LightCollider[i].Activate(false);
		}
	}
}
