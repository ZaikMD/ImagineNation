using UnityEngine;
using System.Collections;

public class AnimatorPlayers : AnimatorController
{
    public enum Animations
    {
        Idle,
        Walk,
        Run,
        Jump,
        Falling,
        Landing,
        Ability,
        Combo_X,
        Combo_XX,
        Combo_XXX,
        Combo_Y,
        Combo_XY,
        Combo_XXY,
        Combo_XXXY,
        Death
    }

    protected override void Start()
    {
        base.Start();

        m_States = new string[]
        {
            "Idle",
            "Walk",
            "Run",
            "Jump",
            "Falling",
            "Landing",
            "Ability",
            "Combo_X",
            "Combo_XX",
            "Combo_XXX",
            "Combo_Y",
            "Combo_XY",
            "Combo_XXY",
            "Combo_XXXY",
            "Death"
        };
    }

    const string COMBO_ = "Combo_";

    int m_LastAnimationPlayed = 0;

    public virtual void playAnimation(Animations animation)
    {
        playAnimation(m_States[(int)animation]);
    }

    public override void playAnimation(int animationNumber)
    {
        playAnimation(m_States[animationNumber]);
    }

    public override void playAnimation(string animationName)
    {
        i_Animator.Play(animationName);
    }

    void requestAnimation(string animation)
    {
        for (int i = 0; i < m_States.Length; i++)
        {
            if(m_States[i].CompareTo(animation) == 0)
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
            //if()
        }
    }
}
