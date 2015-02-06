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
			m_LightCollider[i].enabled = false;
			m_HeavyCollider[i].enabled = false;
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
			m_LightCollider[i].enabled = true;
		}
	}
	
	public override void LightAttackEnd ()
	{
		for (int i = 0; i < m_LightCollider.Length; i++)
		{
			m_LightCollider[i].enabled = false;
		}
	}
	
	public override void HeavyAttackBegin ()
	{
		for (int i = 0; i < m_HeavyCollider.Length; i++)
		{
			m_HeavyCollider[i].enabled = true;
		}
	}
	
	public override void HeavyAttackEnd ()
	{
		for (int i = 0; i < m_HeavyCollider.Length; i++)
		{
			m_HeavyCollider[i].enabled = false;
		}
	}
	
	public override void ConeAttack ()
	{
		Debug.Log ("ConeAttack");
	}
	
	public override void AOEAttack ()
	{
		Debug.Log ("AOEAttack");
	}

	public override void HeavyAOEAttack ()
	{
		Debug.Log ("HeavyAOE");
	}

	public override void LineAttack ()
	{
		Debug.Log ("LineAttack");
	}
}
