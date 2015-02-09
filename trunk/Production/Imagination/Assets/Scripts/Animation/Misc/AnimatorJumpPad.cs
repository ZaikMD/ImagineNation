using UnityEngine;
using System.Collections;

public class AnimatorJumpPad : AnimatorController
{
    const string JUMP = "Jump";
    const float CROSS_FADE_LENGTH = 0.15f;

	// Use this for initialization
	void Start () 
    {
        base.Start();
        m_States = new string[]
        {
            "Jump",
            "Jump2"
        };
	}

    public override void playAnimation(int animationNumber)
    {
        playAnimation();
    }

    public override void playAnimation(string animationName)
    {
        playAnimation();
    }

    public virtual void playAnimation()
    {
        if(!i_Animator.GetCurrentAnimatorStateInfo(0).IsTag(JUMP))
        {
            i_Animator.Play(m_States[Random.Range(0, m_States.Length)]);
        }
        else
        {
            for(int i = 0; i < m_States.Length; i++)
            {
                if(!i_Animator.GetCurrentAnimatorStateInfo(0).IsName(m_States[i]))
                {
                    i_Animator.CrossFade(m_States[i], CROSS_FADE_LENGTH);
                    return;
                }
            }
        }
    }
}
