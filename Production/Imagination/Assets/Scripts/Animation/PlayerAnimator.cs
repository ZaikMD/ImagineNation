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

    public virtual void playAnimation(Animations animationNumber, float fadeLength = DEFAULT_CROSSFADE)
    {
        playAnimation(m_AnimationClips[(int)animationNumber].name, fadeLength);
    }
}
