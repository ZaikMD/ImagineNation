using UnityEngine;
using System.Collections;

public class AlexWeapon : BaseWeapon 
{
	LightCollider m_LightCollider;
	HeavyCollider m_HeavyCollider;

	// Use this for initialization
	void Start () 
	{
		start ();

		m_LightCollider = GetComponentInChildren<LightCollider> ();
		m_HeavyCollider = GetComponentInChildren<HeavyCollider> ();

		m_LightCollider.Activate (false);
		m_LightCollider.SetCharacter (m_ReadInput.ReadInputFrom);

		m_HeavyCollider.Activate (false);
		m_HeavyCollider.SetCharacter (m_ReadInput.ReadInputFrom);
	}
	
	// Update is called once per frame
	void Update () 
	{
		update ();
	}

	public override void LightAttackBegin ()
	{
		m_LightCollider.Activate (true);
	}

	public override void LightAttackEnd ()
	{
		m_LightCollider.Activate (false);
	}

	public override void HeavyAttackBegin ()
	{
		m_HeavyCollider.Activate (true);
	}

	public override void HeavyAttackEnd ()
	{
		m_HeavyCollider.Activate (false);
	}
}
