using UnityEngine;
using System.Collections;

public abstract class BaseWeapon : MonoBehaviour 
{
	bool m_CanCombo = false;
	bool m_ComboFinished = false;
	bool m_AttackFinished = true;

	string m_Input = "Combo_";

	protected abstract void LightAttack();
	protected abstract void HeavyAttack();
	protected abstract void ConeAttack();
	protected abstract void AOEAttack();

	void update()
	{

	}

	void CheckInput()
	{

	}

	void ResetInput()
	{
		m_Input = "Combo_";
	}

	public void ComboTimeStart()
	{
		m_CanCombo = true;
	}

	public void ComboTimeEnd()
	{
		m_CanCombo = true;
	}

	public void AttackOver()
	{
		m_AttackFinished = true;
	}
}
