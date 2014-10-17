using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Created by: Kole Tackney
 * Date: 10, 10, 2014
 * 
 * This script will take request from other scripts to play animations
 * If available it will return that animation to the object calling it.
 * 
 * if not it will pass back the appropriote animation to play.
 * 
 */


//Enum to hold our various states
public enum AnimationStates
{
	Idle,
	LookAround,
	Walk,
	Run,
	Jump,
	Falling,
	Landing,
	Gliding,
	Punch,
	Death,
	TakingWeaponOut,
	DoubleSlash,
	OverHeadSlash
};


public class AnimationState : MonoBehaviour {

	//state varible
	public List<AnimationStates> m_CurrentStates;
	AnimationStates m_CurrentState;
	public float m_AnimTimer;
	public bool m_Grounded;
	public bool m_Jumping;
	public bool m_Attacking;
	public bool m_FinalAttacking;
	float m_AttackTimer = 0.5f;

	//sound manager
	public SFXManager m_SFX;

	//animation clips used to get the length we should play anim
	public AnimationClip m_Jump;
	public AnimationClip m_Punch;
	public AnimationClip m_Slash;
	public AnimationClip m_DoubleSlash;

	// Use this for initialization
	void Start ()
	{
		//Initializing functions that need to be initialized.
		m_CurrentStates = new List<AnimationStates>();
		m_SFX = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SFXManager>();
		m_AnimTimer = m_Jump.length;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//these control timers for animations that need them.
		if(m_Jumping)
		{
			//decrements timer
			m_AnimTimer -= Time.deltaTime;

			//checks if timer is up, if so sets varible
			if(m_AnimTimer < 0)
			{
				m_Jumping = false;
			}	
		}
		if(m_Attacking)
		{
			m_AttackTimer -= Time.deltaTime;

			if(m_AttackTimer < 0)
			{
				m_Attacking = false;
			}	
		}
		if(m_FinalAttacking)
		{
			m_AttackTimer -= Time.deltaTime;
			
			if(m_AttackTimer < 0)
			{
				m_FinalAttacking = false;
			}	
		}
	}

	/// <summary>
	/// Adds the passed in animation to a list, so it can determin if possible at the current time
	/// </summary>
	/// <param name="AnimState">Animation state.</param>
	public void AddAnimRequest(AnimationStates AnimState)
	{
		m_CurrentStates.Add(AnimState);
	}

	/// <summary>
	/// Empties the animation request List.
	/// </summary>
	private void EmptyAnimRequest()
	{
		m_CurrentStates.Clear();	
	}


	/// <summary>
	/// this function goes through a list of the animation requests, and determine which to play
	/// </summary>
	/// <returns>The animation.</returns>
	public string GetAnimation ()
	{
		string currentString = Constants.Animations.IDLE;


		//Cycles through the list of all requested animations, if animation should be played over the current
		//animation, if so replaces current animation with the new one and continues. if it gets to itself it will also continue
		for (int i = 0; i < m_CurrentStates.Count; i++)
		{
			if(m_CurrentStates[i] == AnimationStates.Death)
			{
				//death overides everything, no one can escape death!
				return Constants.Animations.DEATH;
			}
			//checks if grounded, certain animations can only be done in air/on the ground
			if(m_Grounded)
			{
				if(m_CurrentStates[i] == AnimationStates.Jump)
				{
					currentString = Constants.Animations.JUMP;
					startNewAnim(AnimationStates.Jump);
					m_CurrentState = AnimationStates.Jump;
					continue;
				}
				//checks if we currently have a request for double slash, or if we already had one this frame
				else if(m_CurrentStates[i] == AnimationStates.DoubleSlash || currentString == Constants.Animations.DOUBLE_SLASH || m_FinalAttacking)
				{
					//sets are varibles to double slash, so next request can't get farther
					currentString = Constants.Animations.DOUBLE_SLASH;
					startNewAnim(AnimationStates.DoubleSlash);
					m_CurrentState = AnimationStates.DoubleSlash;
					m_FinalAttacking = true;
					//	m_AnimTimer = m_Attack.length;
					continue;
				}
				if(this.gameObject.name == Constants.ALEX_WITH_MOVEMENT_STRING || this.gameObject.name == Constants.ZOE_WITH_MOVEMENT_STRING)
				{
					if(m_CurrentStates[i] == AnimationStates.OverHeadSlash || currentString == Constants.Animations.OVERHEAD_SLASH || m_Attacking)
					{
						currentString = Constants.Animations.OVERHEAD_SLASH;
						m_Attacking = true;
						startNewAnim(AnimationStates.OverHeadSlash);
						m_CurrentState = AnimationStates.OverHeadSlash;
						continue;
					}
				
				}
				if(this.gameObject.name == Constants.DEREK_WITH_MOVEMENT_STRING)
				{
					if(m_CurrentStates[i] == AnimationStates.Punch || currentString == Constants.Animations.RIGHT_HOOK || m_Attacking)
					{
						currentString = Constants.Animations.RIGHT_HOOK;
						m_Attacking = true;
						startNewAnim(AnimationStates.Punch);
						m_CurrentState = AnimationStates.Punch;
						continue;
					}
				}
				if(m_CurrentStates[i] == AnimationStates.Run || currentString == Constants.Animations.RUN)
				{
					currentString = Constants.Animations.RUN;
					startNewAnim(AnimationStates.Run);
					m_CurrentState = AnimationStates.Run;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Walk || currentString == Constants.Animations.WALK)
				{
					currentString = Constants.Animations.WALK;
					startNewAnim(AnimationStates.Walk);
					m_CurrentState = AnimationStates.Walk;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Idle)
				{
					currentString = Constants.Animations.IDLE;
					startNewAnim(AnimationStates.Idle);
					m_CurrentState = AnimationStates.Idle;
					continue;
				}


			}
			else
			{
				if(m_Jumping)
				{
					currentString = Constants.Animations.JUMP;
					startNewAnim(AnimationStates.Jump);
					m_CurrentState = AnimationStates.Jump;
					continue;
				}
				else
				{
					currentString = Constants.Animations.FALLING;
					startNewAnim(AnimationStates.Falling);
					m_CurrentState = AnimationStates.Falling;
					continue;
				}
			}
		}

		EmptyAnimRequest ();
		PlaySound (currentString);
		return currentString;
	}

	/// <summary>
	/// this function checks if we should start a new animation, and sets timers appropriatly
	/// </summary>
	/// <param name="state">State.</param>
	void startNewAnim(AnimationStates state)
	{
		//checks to see if we are doing a main 
		if(state == AnimationStates.Punch || state == AnimationStates.OverHeadSlash)
		{
			if(m_CurrentState != AnimationStates.Punch && state == AnimationStates.Punch)
			{
				m_AttackTimer = m_Punch.length;
				return;
			}
			if(m_CurrentState != AnimationStates.OverHeadSlash && state == AnimationStates.OverHeadSlash)
			{
				m_AttackTimer = m_Slash.length;
				return;
			}
		}

		if(state == AnimationStates.DoubleSlash)
		{
			if(m_CurrentState != AnimationStates.DoubleSlash)
			{
				m_AttackTimer = m_DoubleSlash.length;
				return;
			}
		}


		if(state == AnimationStates.Jump)
		{
			if(m_CurrentState != AnimationStates.Jump)
	        {
				m_AnimTimer = m_Jump.length;
				return;
			}
		}	
	}



	//play sounds for walking, running, idle
	void PlaySound(string currentString)
	{
		switch(currentString)
		{
			//no sound plays while idle
			case Constants.Animations.IDLE:
				m_SFX.stopSound(this.gameObject);
				break;
		
			case Constants.Animations.WALK:
				m_SFX.playSound(this.gameObject, Sounds.Walk);
				break;

			case Constants.Animations.RUN:
				m_SFX.playSound(this.gameObject, Sounds.Run);
				break;

		}
	}
}
