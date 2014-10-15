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

	public AnimationClip m_Jump;

	// Use this for initialization
	void Start ()
	{
		m_CurrentStates = new List<AnimationStates>();

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
				else if(m_CurrentStates[i] == AnimationStates.OverHeadSlash)
				{
					currentString = Constants.Animations.OVERHEAD_SLASH;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Run)
				{
					currentString = Constants.Animations.RUN;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Idle)
				{
					currentString = Constants.Animations.IDLE;
					continue;
				}
				else if(m_CurrentStates[i] == AnimationStates.Walk)
				{
					currentString = Constants.Animations.WALK;
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
		return currentString;
	}




}
