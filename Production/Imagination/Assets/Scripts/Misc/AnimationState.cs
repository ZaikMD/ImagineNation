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

	// Use this for initialization
	void Start ()
	{
		m_CurrentStates = new List<AnimationStates>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_AnimTimer > 0)
		{
			m_AnimTimer -= Time.deltaTime;
		}

	}

	public void AddAnimRequest(AnimationStates AnimState)
	{
		m_CurrentStates.Add(AnimState);
	}

	private void EmptyAnimRequest()
	{
		m_CurrentStates.Clear();	
	}

	public string GetAnimation ()
	{
		if(m_Grounded)
		{
			return Constants.Animations.IDLE;
		}
		else
		{
			return Constants.Animations.FALLING;
		}
	}


	public string PlayAnimation(string AnimationToPlay)
	{
		//GO to see if we can play sound this sound

		//If jump
		switch(AnimationToPlay)
		{
		
		case Constants.Animations.IDLE:
			{
				//if(m_CurrentState == AnimationStates.Jump || m_CurrentState == AnimationStates.Falling || m_CurrentState == AnimationStates.Gliding)

					return Constants.Animations.IDLE;

				break;
			}

		case Constants.Animations.WALK:
			{
				
				return Constants.Animations.WALK;
				break;
			}

		case Constants.Animations.RUN:
			{
				return Constants.Animations.RUN;
				break;
			}

		case Constants.Animations.JUMP:
			{
				//check if we can jump
			//	m_CurrentState = AnimationStates.Jump;
				return Constants.Animations.JUMP;
				break;
			}

		case Constants.Animations.FALLING:
			{
				//check if we can fall

					return Constants.Animations.FALLING;
					break;
			}

		case Constants.Animations.GLIDING:
			{
				//check if we can fall
				return Constants.Animations.GLIDING;
				break;
			}

		case Constants.Animations.DEATH:
			{
				return Constants.Animations.DEATH;
				break;
			}
		
		case Constants.Animations.DOUBLE_SLASH:
			{
				return Constants.Animations.DOUBLE_SLASH;
				break;
			}
		case Constants.Animations.RIGHT_HOOK:
			{
				return Constants.Animations.RIGHT_HOOK;
				break;
			}	

		case Constants.Animations.OVERHEAD_SLASH:
			{
				return Constants.Animations.OVERHEAD_SLASH;
				break;
			}
		
		case Constants.Animations.LOOK_AROUND:
			{
				return Constants.Animations.LOOK_AROUND;
				break;
			}
		
		case Constants.Animations.TAKING_WEAPON_OUT:
			{
				return Constants.Animations.TAKING_WEAPON_OUT;
				break;
			}				
		
		default:
		{
#if UNITY_EDITOR || DEBUG
			Debug.LogError("Passed in value is unknown, are you using Constants.Animations?");
#endif
			return Constants.Animations.IDLE;
			break;
		}


	}

	}





}
