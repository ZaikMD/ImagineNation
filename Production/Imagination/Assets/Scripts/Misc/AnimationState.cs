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

	public List<AnimationStates> m_CurrentStates;
	public float m_AnimTimer;
	public bool m_Grounded;
	public bool m_Jumping;
	public bool m_Attacking;
	public bool m_FinalAttacking;

	float m_AttackTimer = 0.5f;

	public SFXManager m_SFX;
	public AnimationClip m_Jump;


	// Use this for initialization
	void Start ()
	{
		m_CurrentStates = new List<AnimationStates>();
		m_SFX = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SFXManager>();
		m_AnimTimer = m_Jump.length;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Jumping)
		{
			m_AnimTimer -= Time.deltaTime;

			if(m_AnimTimer < 0)
			{
				m_Jumping = false;
				m_AnimTimer = m_Jump.length;
			}	
		}

		if(m_Attacking)
		{
			m_AttackTimer -= Time.deltaTime;

			if(m_AttackTimer < 0)
			{
				m_Attacking = false;
				m_AttackTimer = 1.0f;
			}	
		}

		if(m_FinalAttacking)
		{
			m_AttackTimer -= Time.deltaTime;
			
			if(m_AttackTimer < 0)
			{
				m_FinalAttacking = false;
				m_AttackTimer = 1.0f;
			}	
		}

	}

	/// <summary>
	/// Adds a request to play an animation, if 
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
	/// to use, 
	/// </summary>
	/// <returns>The animation.</returns>
	public string GetAnimation ()
	{
		string currentString = Constants.Animations.IDLE;

		for (int i = 0; i < m_CurrentStates.Count; i++)
		{
			if(m_CurrentStates[i] == AnimationStates.Death)
			{
				//death overides everything, no one can escape death!
				return Constants.Animations.DEATH;
			}
			if(m_Grounded)
			{
				if(m_CurrentStates[i] == AnimationStates.Jump)
				{
					currentString = Constants.Animations.JUMP;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.DoubleSlash || currentString == Constants.Animations.DOUBLE_SLASH || m_FinalAttacking)
				{
					currentString = Constants.Animations.DOUBLE_SLASH;
					m_FinalAttacking = true;
					//	m_AnimTimer = m_Attack.length;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.OverHeadSlash || currentString == Constants.Animations.OVERHEAD_SLASH || m_Attacking)
				{
					currentString = Constants.Animations.OVERHEAD_SLASH;
					m_Attacking = true;
				//	m_AnimTimer = m_Attack.length;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Punch || currentString == Constants.Animations.RIGHT_HOOK)
				{
					currentString = Constants.Animations.RIGHT_HOOK;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Run || currentString == Constants.Animations.RUN)
				{
					currentString = Constants.Animations.RUN;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Walk || currentString == Constants.Animations.WALK)
				{
					currentString = Constants.Animations.WALK;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Idle)
				{
					currentString = Constants.Animations.IDLE;
					continue;
				}


			}
			else
			{
				if(m_Jumping)
				{
					currentString = Constants.Animations.JUMP;
					continue;
				}
				else
				{
					currentString = Constants.Animations.FALLING;
					continue;
				}
			}
		}

		EmptyAnimRequest ();
		PlaySound (currentString);
		return currentString;
	}


	void PlaySound(string currentString)
	{
		switch(currentString)
		{
			case Constants.Animations.IDLE:
				m_SFX.stopSound(this.gameObject);
				break;
		
			case Constants.Animations.WALK:
				m_SFX.playSound(this.gameObject, Sounds.Walk);
				break;

			case Constants.Animations.RUN:
				m_SFX.playSound(this.gameObject, Sounds.Run);
				break;
	/*
			case Constants.Animations.JUMP:
				switch(this.gameObject.name)
				{
					case Constants.ALEX_WITH_MOVEMENT_STRING:
					m_SFX.playSound(this.gameObject, Sounds.AlexJump);
					break;
					case Constants.DEREK_WITH_MOVEMENT_STRING:
					m_SFX.playSound(this.gameObject, Sounds.DerekJump);
					break;
					case Constants.ZOE_WITH_MOVEMENT_STRING:
					m_SFX.playSound(this.gameObject, Sounds.ZoeyJump);
					break;

				}
	*/			break;
		}
	}
}
