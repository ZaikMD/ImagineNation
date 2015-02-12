using UnityEngine;
using System.Collections;

public abstract class BaseWeapon : MonoBehaviour, CallBack
{
	bool m_CanCombo = true;
	bool m_ComboSet = false;

	bool m_ComboFinished = false;
	bool m_AttackFinished = true;

	AnimatorPlayers m_Animator;
	BaseMovementAbility m_Movement;

	//Projectile Variables(For Attacking)
	public GameObject m_LightColliderPrefab;
	public GameObject m_HeavyColliderPrefab;

	protected Vector3 m_InitialProjectilePosition;
	protected Vector3 m_InitialProjectileRotation;
	protected Quaternion m_ProjectileRotation;
	protected float m_FirePointOffset = 0.5f;

	//AOE 
	public int m_NumberOfAOEProjectiles = 8;
	protected float m_AOEProjectileAngle;
	public int m_AOERange = 2;
	public int m_AOESpeed = 10;

	//Line
	public int m_LineRange = 6;
	public int m_LineSpeed = 10;

	//Cone
	public int m_NumberOfConeProjectiles = 6;
	public int m_ConeAngle = 15;
	public int m_ConeRange = 4;
	public int m_ConeSpeed = 10;
	int m_ConeAngleOffset;



	//The constants for the inputs of the attacks, as well as the 
	//combos the players can do. L = light attack  H = Heavy attack

	const string X = "X";
	const string XXX = "XXX";
	const string Y = "Y";
	const string STRING_RESET = "Combo_";

	string m_Input = STRING_RESET;
	string m_LastInput;

	AcceptInputFrom m_ReadInput;  //To get the input


	protected void start()
	{
		m_Animator = GetComponentInParent<AnimatorPlayers> ();
		m_Movement = GetComponentInParent<BaseMovementAbility> ();
		m_ReadInput = GetComponentInParent<AcceptInputFrom>();

		m_AOEProjectileAngle = 360 / m_NumberOfAOEProjectiles;

		int numb = m_NumberOfConeProjectiles / 2;
		m_ConeAngleOffset = numb * m_ConeAngle;
		GetComponent<AnimationCallBackManager> ().registerCallBack (this);
	}

	protected virtual void update()
	{
		CheckInput ();

		//If the last attack is finished and we have selected the next attack
		if (m_AttackFinished && m_ComboSet)
		{
			//Play the animation
			m_Animator.playAnimation(m_Input);
			//Flag that we have not selected our next move and we have not finished our attack
			m_ComboSet = false;
			m_AttackFinished = false;

			//Tell the movement it cant jump anymore
			m_Movement.CanJump(false);
			m_Movement.m_PausedMovement = true;

			//If our combo is over then reset the input
			if (m_ComboFinished )		  
				ResetInput ();
		}

		//If our attack is finished and we have not set a new input by now set the can combo to true
		//This is here so you can start up a new combo after not having attacked
			if (m_AttackFinished && !m_ComboSet)
				m_CanCombo = true;

	}

	void CheckInput()
	{
		//If we can combo and we have not already selected the next input and we are on the ground
		if (m_CanCombo && !m_ComboSet && m_Movement.GetIsGrounded()) 
		{
			// If we have pressed any of the attack buttons
			if (InputManager.getAttackDown(m_ReadInput.ReadInputFrom) || InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))
			{
				//If we have pressed X add that to the input string 
				if(InputManager.getAttackDown(m_ReadInput.ReadInputFrom))
					m_LastInput = X;			

				//If we have pressed Y add that to the input string
				if(InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))			
					m_LastInput = Y;

				//If we have pressed Y then its time to end a combo
				if (m_LastInput == Y )
					m_ComboFinished = true;

				// If we have pressed 3 x and a y then its time to end the combo
				else if (m_Input.Contains(XXX) && m_LastInput == X)	
					ResetInput();

				// Add the last inout into the input string
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

	void FootStep()
	{
		m_Movement.m_PausedMovement = true;
		m_Movement.SetForcedInput(transform.forward);
		m_Movement.SetSpeedMultiplier (0.15f);
	}

    public void CallBack(CallBackEvents callBack)
	{
		switch(callBack)
		{
			case CallBackEvents.Player_ComboTimeStart:
				ComboTimeStart();
				break;

			case CallBackEvents.FootStep: 
				FootStep();
				break;

			case CallBackEvents.Player_AttackBegin_Light:
				LightAttackBegin();
				break;
			
			case CallBackEvents.Player_AttackBegin_Heavy:
				HeavyAttackBegin();
				break;
			
			case CallBackEvents.Player_AttackBegin_AOE:
				AOEAttack();
				break;
			
			case CallBackEvents.Player_AttackBegin_HeavyAOE:
				HeavyAOEAttack();
				break;
			
			case CallBackEvents.Player_AttackBegin_Line:
				LineAttack ();
				break;
			
			case CallBackEvents.Player_AttackBegin_Cone:
				ConeAttack();
				break;
			
			case CallBackEvents.Player_ComboTimeEnd:
				ComboTimeEnd();
				break;
			
			case CallBackEvents.Player_AttackOver:
				AttackOver();
				break;
		}
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
		m_Movement.SetForcedInput(Vector3.zero);
		m_Movement.SetSpeedMultiplier (1f);
		m_AttackFinished = true;
		m_Movement.CanJump(true);
		LightAttackEnd ();
		HeavyAttackEnd ();
	}

	public abstract void LightAttackBegin();
	public abstract void LightAttackEnd();
	public abstract void HeavyAttackBegin();
	public abstract void HeavyAttackEnd();
	
	public virtual void ConeAttack()
	{
		m_InitialProjectilePosition = transform.position;
		m_InitialProjectileRotation = transform.rotation.eulerAngles;
		m_InitialProjectileRotation.y -= m_ConeAngleOffset;

		
		//offet this to be at one end of the cone		
		for(int i = 0; i < m_NumberOfConeProjectiles; i++)
		{
			m_ProjectileRotation = Quaternion.Euler(m_InitialProjectileRotation.x, m_InitialProjectileRotation.y + (i * m_ConeAngle), m_InitialProjectileRotation.z);
			//Create projectiles 
			GameObject proj =  (GameObject)GameObject.Instantiate (m_LightColliderPrefab,
			                                                       new Vector3(m_InitialProjectilePosition.x, 
			            										   m_InitialProjectilePosition.y + m_FirePointOffset, 
			           											   m_InitialProjectilePosition.z),
			                                                       m_ProjectileRotation);
			proj.GetComponent<LightCollider>().LaunchProjectile(m_ConeSpeed, m_ConeRange);
		}
	}
	
	public virtual void AOEAttack()
	{
		m_InitialProjectilePosition = transform.position;
		m_InitialProjectileRotation = transform.rotation.eulerAngles;
		
		for(int i = 0; i < m_NumberOfAOEProjectiles; i++)
		{
			m_ProjectileRotation = Quaternion.Euler(m_InitialProjectileRotation.x, m_InitialProjectileRotation.y + (i * m_AOEProjectileAngle), m_InitialProjectileRotation.z);
			GameObject proj =  (GameObject)GameObject.Instantiate (m_LightColliderPrefab,
			                                                       new Vector3(m_InitialProjectilePosition.x,
			           											   m_InitialProjectilePosition.y + m_FirePointOffset,
			            										   m_InitialProjectilePosition.z), m_ProjectileRotation);
			
			proj.GetComponent<LightCollider>().LaunchProjectile(m_AOESpeed,m_AOERange);
		}
	}
	
	public virtual void HeavyAOEAttack()
	{
		m_InitialProjectilePosition = transform.position;
		m_InitialProjectileRotation = transform.rotation.eulerAngles;
		
		for(int i = 0; i < m_NumberOfAOEProjectiles; i++)
		{
			m_ProjectileRotation = Quaternion.Euler(m_InitialProjectileRotation.x, m_InitialProjectileRotation.y + (i * m_AOEProjectileAngle), m_InitialProjectileRotation.z);
			GameObject proj =  (GameObject)GameObject.Instantiate (m_HeavyColliderPrefab,
			                                                       new Vector3(m_InitialProjectilePosition.x,
			           											   m_InitialProjectilePosition.y + m_FirePointOffset,
			           											   m_InitialProjectilePosition.z), m_ProjectileRotation);
			
			proj.GetComponent<HeavyCollider>().LaunchProjectile(m_AOESpeed,m_AOERange);
		}
	}
	
	public virtual void LineAttack ()
	{
		m_InitialProjectilePosition = transform.position;
		m_InitialProjectileRotation = transform.rotation.eulerAngles;
		
		GameObject proj =  (GameObject)GameObject.Instantiate (m_HeavyColliderPrefab,
		                                                       new Vector3(m_InitialProjectilePosition.x,
		           											   m_InitialProjectilePosition.y + m_FirePointOffset,
		           											   m_InitialProjectilePosition.z), Quaternion.Euler(m_InitialProjectileRotation));
		
		proj.GetComponent<HeavyCollider>().LaunchProjectile(m_LineSpeed,m_LineRange);
	}

}
