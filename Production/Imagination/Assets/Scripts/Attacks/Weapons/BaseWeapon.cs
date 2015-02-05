using UnityEngine;
using System.Collections;

public abstract class BaseWeapon : MonoBehaviour 
{
	bool m_CanCombo = true;
	bool m_ComboSet = false;

	bool m_ComboFinished = false;
	bool m_AttackFinished = true;

	AnimatorPlayers m_Animator;

	//The constants for the inputs of the attacks, as well as the 
	//combos the players can do. L = light attack  H = Heavy attack

	const string X = "X";
	const string XX = "XX";
	const string Y = "Y";
	const string STRING_RESET = "Combo_";

	string m_Input = STRING_RESET;
	string m_LastInput;

	AcceptInputFrom m_ReadInput;  //To get the input

	public abstract void LightAttackBegin();
	public abstract void LightAttackEnd();
	public abstract void HeavyAttackBegin();
	public abstract void HeavyAttackEnd();
	public abstract void ConeAttack();
	public abstract void AOEAttack();

	protected void start()
	{
		m_Animator = GetComponentInParent<AnimatorPlayers> ();
		m_ReadInput = GetComponentInParent<AcceptInputFrom>();
	}

	protected virtual void update()
	{
		CheckInput ();

		if (m_AttackFinished && m_ComboSet)
		{
			m_Animator.playAnimation(m_Input);
			m_ComboSet = false;
			m_AttackFinished = true;

			if (m_ComboFinished )		  
				ResetInput ();
		}

			if (m_AttackFinished && !m_ComboSet)
				m_CanCombo = true;

	}

	void CheckInput()
	{
		if (m_CanCombo && !m_ComboSet) 
		{
			if (InputManager.getAttackDown(m_ReadInput.ReadInputFrom) || InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))
			{
				if(InputManager.getAttackDown(m_ReadInput.ReadInputFrom))
					m_LastInput = X;			
				
				if(InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))			
					m_LastInput = Y;
				
				if (m_LastInput == Y || m_Input.Contains(XX) && m_LastInput == X)
					m_ComboFinished = true;

				
				m_Input += m_LastInput;
				m_ComboSet = true;
				m_CanCombo = false;
			}
		}
	}

	void ResetInput()
	{
		m_Input = STRING_RESET;
		m_ComboFinished = false;
	}

	public void ComboTimeStart()
	{
		m_CanCombo = true;
	}

	public void ComboTimeEnd()
	{
		m_CanCombo = false;

		if (!m_ComboSet)
			ResetInput ();
	}

	public void AttackOver()
	{
		m_AttackFinished = true;
		LightAttackEnd ();
	}
}
