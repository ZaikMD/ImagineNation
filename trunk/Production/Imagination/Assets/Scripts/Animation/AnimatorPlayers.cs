using UnityEngine;
using System.Collections;

public class AnimatorPlayers : AnimatorController
{
    public enum Animations
    {
        Idle,
        Run,
        Jump,
        Falling,
        Landing,
        Ability,
		Combo_X,
		Combo_XX,
		Combo_X_Air,
		Combo_X_Air_Loop,
		Combo_Y_Start,
		Combo_Y,
        Death,
		Combo_Blank
    }

    protected override void Start()
    {
        base.Start();

        m_States = new string[]
        {
            "Idle",
            "Run",
            "Jump",
            "Falling",
            "Landing",
            "Ability",
            "Combo_X",
            "Combo_XX",
            "Combo_X_Air",
            "Combo_X_Air_Loop",
            "Combo_Y_Start",
            "Combo_Y",
            "Death",
			"Combo_Blank"
        };
    }

    const string COMBO_ = "Combo_";
    const string ATTACK = "Attack";
	const string NORMALIZED_SPEED = "NormalizedSpeed";

    int m_LastAnimationPlayed = 0;

    public virtual void playAnimation(Animations animation)
    {
        playAnimation((int)animation);
    }

    public override void playAnimation(int animationNumber)
    {
        requestAnimation(animationNumber);
    }

    public override void playAnimation(string animationName)
    {
        requestAnimation(animationName);
    }

    void requestAnimation(string animation)
    {
        for (int i = 0; i < m_States.Length; i++)
        {
            if(m_States[i].Equals(animation, System.StringComparison.OrdinalIgnoreCase))
            {
                requestAnimation(i);
                return;
            }
        }
    }

    void requestAnimation(int animation)
    {
        if (!m_States[animation].Contains(COMBO_))
        {
            //if (!i_Animator.GetCurrentAnimatorStateInfo(0).IsTag(ATTACK))
            {
                i_Animator.Play(m_States[animation]);
                m_LastAnimationPlayed = animation;
            }
        }
        else
        {
            i_Animator.Play(m_States[animation], 1 , 0);
            m_LastAnimationPlayed = animation;
        }        
    }

	public void setMoveSpeed(float normalizedSpeed)
	{
		i_Animator.SetFloat (NORMALIZED_SPEED, normalizedSpeed);
	}
}
