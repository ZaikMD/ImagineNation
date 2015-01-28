using UnityEngine;
using System.Collections;

public class PlayerAnimator : BaseAnimator
{
    public enum Animations
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

    public virtual void playAnimation(Animations animationNumber, float fadeLength = DEFAULT_FADE)
    {
        playAnimation(m_AnimationClips[(int)animationNumber].name, fadeLength);
    }

	public virtual void addAnimation(Animations animationNumber, float targetWeight = DEFAULT_WEIGHT, float fadeLength = DEFAULT_FADE)
	{
		addAnimation(m_AnimationClips[(int)animationNumber].name, targetWeight, fadeLength);
	}
}
