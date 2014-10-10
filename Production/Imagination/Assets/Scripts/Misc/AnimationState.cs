using UnityEngine;
using System.Collections;

/*
 * Created by: Kole Tackney
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

	AnimationStates m_CurrentState;
	float m_AnimTimer;

	// Use this for initialization
	void Start ()
	{
	


	}
	
	// Update is called once per frame
	void Update () 
	{


	
	}


	public string PlayAnimation(string AnimationToPlay)
	{
		//GO to see if we can play sound this sound

		//If jump
		switch(AnimationToPlay)
		{
		
		case Constants.Animations.IDLE:
			{
				
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
				m_CurrentState = AnimationStates.Jump;
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
