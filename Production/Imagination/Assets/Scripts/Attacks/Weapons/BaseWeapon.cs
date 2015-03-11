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
	public float m_MoveSpeedPercentageWhileAttacking = 0.8f;

	//Regular Attack
	public float m_AttackSpeed = 7.0f;
	public float m_AttackRange = 1.0f;
	public float m_AttackSpread = 25.0f;
	public int m_AttackProjectileNumb = 6;
	public float m_AttackStartingRot = 50.0f;

	// Charging variables
	protected bool m_Charging = false;
	protected float m_ChargeTimer;
	public float m_MinChargeTime = 1.0f;
	public float m_MaxChargeTime = 3.0f;
	public float m_MoveSpeedPercentageWhileCharging = 0.4f;
	
	//Air Slam variables
	protected bool m_Slamming = false;
	public float m_MinDistToSlam = 0.75f;
	
	//AOE 
	public int m_NumberOfAOEProjectiles = 8;
	protected float m_AOEProjectileAngle;
	public int m_AOERange = 2;
	public int m_AOESpeed = 10;

	//Safety timer so incase the call back function doesnt get called
	const float m_SafetyTime = 1.0f;
	float m_AttackSafetyTimer = 0.0f;

	//Particle effect variables	

	public GameObject[] m_AOEEffects;
	public GameObject[] m_ChargingEffectPrefabs;
	protected GameObject[] m_ChargingEffectObject;
	public GameObject[] m_ChargedEffectPrefabs;
	protected GameObject[] m_ChargedEffectObject;
	protected TrailRenderer[] m_TrailRenderers;
	protected bool m_ChargeGlowOn = false;

	
	//The constants for the inputs of the attacks, as well as the 
	//combos the players can do. L = light attack  H = Heavy attack
	
	protected const string X = "X";
	protected const string XX = "XX";
	const string Y = "Y";
	const string AX = "X_Air";
	const string STRING_RESET = "Combo_";
	
	protected string m_Input = STRING_RESET;
	string m_LastInput;
	
	protected AcceptInputFrom m_ReadInput;  //To get the input
	
	protected void start()
	{
		m_Animator = GetComponentInParent<AnimatorPlayers> ();
		m_Movement = GetComponentInParent<BaseMovementAbility> ();
		//m_ReadInput = GetComponentInParent<AcceptInputFrom>();
		m_ReadInput = transform.parent.GetComponent<AcceptInputFrom> ();


		m_AOEProjectileAngle = 360 / m_NumberOfAOEProjectiles;
		
		GetComponent<AnimationCallBackManager> ().registerCallBack (this);

		m_TrailRenderers = GetComponentsInChildren<TrailRenderer>();
	}
	
	protected virtual void update()
	{	
		
		CheckInput ();

		if (m_Charging)
		{
			// If we have surpassed max charge time then stop charging	
			m_ChargeTimer += Time.deltaTime;
			if (m_ChargeTimer >= m_MinChargeTime)
				ChargedEffect();

			if (m_ChargeTimer >= m_MaxChargeTime)
			{
				m_Charging = false;
				m_AttackFinished = true;
				RemoveChargingEffects();
			}
			//If we are not still holding the charge button then check if we have surpassed the minimum charge time
			else if (InputManager.getHeavyAttackUp(m_ReadInput.ReadInputFrom))
			{
				if (m_ChargeTimer > m_MinChargeTime)
				{
					m_Charging = false;
					m_AttackFinished = true;
					RemoveChargingEffects();
				}
				else
					Reset();
			}

			return;
		}

		//This is here just to make sure we dont do the slam animation too early, so if we are not grounded yet we will just not continue the logic
		// If we are in the middle of an air slam
		if (m_Slamming)
		{
			RaycastHit hitInfo;
			if (!Physics.Raycast(transform.position, Vector3.down, out hitInfo))
				return;

			if (hitInfo.distance < 1.0f)
				m_Slamming = false; 
			else 
				return;
		}
		
		//If the last attack is finished and we have selected the next attack
		if (m_AttackFinished && m_ComboSet)
		{
			TurnOnTrail();
			//Play the animation
			m_Animator.playAnimation(m_Input);
			//Flag that we have not selected our next move and we have not finished our attack
			m_ComboSet = false;
			m_AttackFinished = false;
			
			//Tell the movement it cant jump anymore
			m_Movement.CanJump(false);		

			//Reset the Safety timer
			m_AttackSafetyTimer = m_SafetyTime;

			//If our combo is over then reset the input
			if (m_ComboFinished )		  
				ResetInput ();
		}
		
		//If our attack is finished and we have not set a new input by now set the can combo to true
		//This is here so you can start up a new combo after not having attacked
		if (m_AttackFinished && !m_ComboSet)
			m_CanCombo = true;

		if (!m_AttackFinished)
		{
			m_AttackSafetyTimer -= Time.deltaTime;
			if (m_AttackSafetyTimer < 0.0f)
				AttackOver();
		}
		
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
				m_Movement.SetSpeedMultiplier(m_MoveSpeedPercentageWhileAttacking);

				if (m_Input.Contains(Y))
					ResetInput();
			}
			
			//If we have pressed Y add that to the input string and set charging to true
			else if(InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))	
			{
				m_LastInput = Y;
				m_Charging = true;	
				m_ChargeTimer = 0.0f;	
				m_Movement.SetSpeedMultiplier(m_MoveSpeedPercentageWhileCharging);
				m_Movement.CanJump(false);	
				m_Animator.playAnimation(AnimatorPlayers.Animations.Combo_Y_Start);
				ChargingEffect();
				
				if (m_Input.Contains(Y) || m_Input.Contains(X))
					ResetInput();
			}
			
			// If we have pressed 2 x then reset the input
			if (m_Input.Contains(XX))	
					ResetInput();
		}

		else if(!m_Movement.GetIsGrounded())
		{
			if(InputManager.getAttackDown(m_ReadInput.ReadInputFrom))
			{
				RaycastHit hitInfo;
				if (!Physics.Raycast(transform.position, Vector3.down, out hitInfo))
				    return;

				if(hitInfo.distance >= m_MinDistToSlam)
				{
					m_LastInput = AX;
					m_Slamming = true;
					m_Movement.IsAirAttacking = true;
					m_Animator.playAnimation(AnimatorPlayers.Animations.Combo_X_Air_Loop);
				}
			}
		}

		if (m_LastInput != null)
		{
			if (m_Input.Contains(AX))
				ResetInput();

			// Add the last inout into the input string
			m_Input += m_LastInput;
			m_LastInput = null;
			m_ComboSet = true;
			m_CanCombo = false;
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
	
	void Reset()
	{
		ResetInput ();
		m_Charging = false;
		m_ComboSet = false;
		m_CanCombo = true;
		m_AttackFinished = true;
		m_Movement.SetSpeedMultiplier(1.0f);
		m_Movement.setMovementPaused (false);
		m_Movement.CanJump(true);	
		m_Animator.playAnimation(AnimatorPlayers.Animations.Combo_Blank);
		RemoveChargingEffects();
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
		Debug.Log ("AttackOver");
		m_AttackFinished = true;
		m_Movement.SetSpeedMultiplier(0.75f);
		m_Movement.CanJump(true);
		m_Movement.setMovementPaused (false);
		TurnOffTrail();
		m_Slamming = false;
	}
	
	public virtual void AttackBegin()
	{
		m_InitialProjectilePosition = transform.position;
		m_InitialProjectileRotation = transform.rotation.eulerAngles;
		m_InitialProjectileRotation.y -= m_AttackStartingRot;
		
		for(int i = 0; i < m_AttackProjectileNumb; i++)
		{
			m_ProjectileRotation = Quaternion.Euler(m_InitialProjectileRotation.x, m_InitialProjectileRotation.y + (i * m_AttackSpread ), m_InitialProjectileRotation.z);
			GameObject proj =  (GameObject)GameObject.Instantiate (m_HeavyColliderPrefab,
			                                                       new Vector3(m_InitialProjectilePosition.x,
			            										   m_InitialProjectilePosition.y + m_FirePointOffset,
			            										   m_InitialProjectilePosition.z), m_ProjectileRotation);
			
			HeavyCollider collider = proj.GetComponent<HeavyCollider>();
			collider.LaunchProjectile(7,1);
			collider.SetCharacter(m_ReadInput.ReadInputFrom);
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
			
			LightCollider collider = proj.GetComponent<LightCollider>();
			collider.LaunchProjectile(m_AOESpeed,m_AOERange);
			collider.SetCharacter(m_ReadInput.ReadInputFrom);
			AOEEffect();
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
		AOEEffect();
	}

	protected void TurnOnTrail()
	{
		for (int i = 0; i < m_TrailRenderers.Length; i++)
		{
			m_TrailRenderers[i].enabled = true;
		}
	}

	protected void TurnOffTrail()
	{
		for (int i = 0; i < m_TrailRenderers.Length; i++)
		{
			m_TrailRenderers[i].enabled = false;
		}
	}

	protected abstract void AOEEffect();
	protected abstract void ChargingEffect();
	protected abstract void ChargedEffect();
	protected abstract void RemoveChargingEffects();
}