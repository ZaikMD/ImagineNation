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
	
	// Charging variables
	protected bool m_Charging = false;
	protected float m_ChargeTimer;
	public float m_MinChargeTime = 1.0f;
	public float m_MaxChargeTime = 3.0f;
	
	//AOE 
	public int m_NumberOfAOEProjectiles = 8;
	protected float m_AOEProjectileAngle;
	public int m_AOERange = 2;
	public int m_AOESpeed = 10;
	
	
	//The constants for the inputs of the attacks, as well as the 
	//combos the players can do. L = light attack  H = Heavy attack
	
	const string X = "X";
	const string XX = "XX";
	const string Y = "Y";
	const string STRING_RESET = "Combo_";
	
	string m_Input = STRING_RESET;
	string m_LastInput;
	
	protected AcceptInputFrom m_ReadInput;  //To get the input
	
	protected void start()
	{
		m_Animator = GetComponentInParent<AnimatorPlayers> ();
		m_Movement = GetComponentInParent<BaseMovementAbility> ();
		m_ReadInput = GetComponentInParent<AcceptInputFrom>();
		
		m_AOEProjectileAngle = 360 / m_NumberOfAOEProjectiles;
		
		GetComponent<AnimationCallBackManager> ().registerCallBack (this);
	}
	
	protected virtual void update()
	{	
		
		CheckInput ();

		if (m_Charging)
		{
			// If we have surpassed max charge time then stop charging	
			m_ChargeTimer += Time.deltaTime;
			if (m_ChargeTimer >= m_MaxChargeTime)
			{
				m_Charging = false;
				m_AttackFinished = true;
			}
			//If we are not still holding the charge button then check if we have surpassed the minimum charge time
			else if (InputManager.getHeavyAttackUp(m_ReadInput.ReadInputFrom))
			{
				if (m_ChargeTimer > m_MinChargeTime)
				{
					m_Charging = false;
					m_AttackFinished = true;
				}
				else
					LeaveCharge();
			}

			return;
		}
		
		//If the last attack is finished and we have selected the next attack
		if (m_AttackFinished && m_ComboSet)
		{
			//Play the animation
			m_Animator.playAnimation(m_Input);
			Debug.Log(m_Input + ": AttackExecuted");
			//Flag that we have not selected our next move and we have not finished our attack
			m_ComboSet = false;
			m_AttackFinished = false;
			
			//Tell the movement it cant jump anymore
			m_Movement.CanJump(false);		
			
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
		if (!m_CanCombo || m_ComboSet)
			return;
		// If we haven't pressed any of the attack buttons then return;
		if (!InputManager.getAttackDown(m_ReadInput.ReadInputFrom) && !InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))
			return;
		
		if(m_Movement.GetIsGrounded())
		{
			//If we have pressed X add that to the input string 
			if(InputManager.getAttackDown(m_ReadInput.ReadInputFrom))
			{
				m_LastInput = X;	
				if (m_Input.Contains(Y))
					ResetInput();
			}
			
			//If we have pressed Y add that to the input string and set charging to true
			else if(InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))	
			{
				m_LastInput = Y;
				Debug.Log ("Charging = true");
				m_Charging = true;	
				m_ChargeTimer = 0.0f;	
				m_Movement.m_PausedMovement = true;
				m_Movement.CanJump(false);	
				
				if (m_Input.Contains(Y) || m_Input.Contains(X))
					ResetInput();
			}
			
			// If we have pressed 2 x then reset the input
			if (m_Input.Contains(XX))	
					ResetInput();

			Debug.Log("Registered Input: " + m_LastInput);
			// Add the last inout into the input string
			m_Input += m_LastInput;
			m_ComboSet = true;
			m_CanCombo = false;
			if (m_Input.Contains(Y))
				m_Animator.playAnimation(AnimatorPlayers.Animations.Combo_Y_Start);

			Debug.Log("Actual Input: " + m_Input);
		}
		
		
	}
	
	void ResetInput()
	{
		m_Input = STRING_RESET;
		m_ComboFinished = false;
	}
	
	public void CallBack(CallBackEvents callBack)
	{
		switch(callBack)
		{
		case CallBackEvents.Player_ComboTimeStart:
			ComboTimeStart();
			break;

		case CallBackEvents.Player_AttackBegin:
			AttackBegin();
			break;
			
		case CallBackEvents.Player_AttackBegin_AOE:
			AOEAttack();
			break;
			
		case CallBackEvents.Player_AttackBegin_HeavyAOE:
			HeavyAOEAttack();
			break;
			
		case CallBackEvents.Player_ComboTimeEnd:
			ComboTimeEnd();
			break;
			
		case CallBackEvents.Player_AttackOver:
			AttackOver();
			break;
		}
	}
	
	void LeaveCharge()
	{
		Debug.Log ("LeftCharge");
		m_Charging = false;
		ResetInput ();
		m_ComboSet = false;
		m_CanCombo = true;
		m_AttackFinished = true;
		m_Movement.setMovementPaused (false);
		m_Movement.CanJump(true);	
		m_Animator.playAnimation(AnimatorPlayers.Animations.Idle);
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
		m_Movement.CanJump(true);
		m_Movement.setMovementPaused (false);
		AttackEnd ();
	}
	
	public abstract void AttackBegin();
	public abstract void AttackEnd();
	
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
			
			LightCollider collider = proj.GetComponent<LightCollider>();
			collider.LaunchProjectile(m_AOESpeed,m_AOERange);
			collider.SetCharacter(m_ReadInput.ReadInputFrom);
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
			
			HeavyCollider collider = proj.GetComponent<HeavyCollider>();
			collider.LaunchProjectile(m_AOESpeed,m_AOERange);
			collider.SetCharacter(m_ReadInput.ReadInputFrom);
		}
	}
}