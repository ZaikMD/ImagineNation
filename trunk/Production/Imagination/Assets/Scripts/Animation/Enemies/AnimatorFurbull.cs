using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorFurbull : AnimatorEnemyBase 
{
    public enum Animations
    {
        Idle,
        Run,
        Attack
    };

	// Use this for initialization
	protected override void Start () 
    {
        base.Start();

        m_States = new string[]
        {
            "Idle",
            "Run",
            "Attack"
        };


        m_StatesDitctionary.Add(m_States[(int)Animations.Idle], new List<string>());

        m_StatesDitctionary[m_States[(int)Animations.Idle]].Add("Idle");
        m_StatesDitctionary[m_States[(int)Animations.Idle]].Add("HeadTurn");
        m_StatesDitctionary[m_States[(int)Animations.Idle]].Add("Breathing");
        m_StatesDitctionary[m_States[(int)Animations.Idle]].Add("FeetStamp");

        m_StatesDitctionary.Add(m_States[(int)Animations.Run], new List<string>());

        m_StatesDitctionary[m_States[(int)Animations.Run]].Add("Run Start");

        m_StatesDitctionary.Add(m_States[(int)Animations.Attack], new List<string>());

        m_StatesDitctionary[m_States[(int)Animations.Attack]].Add("Head Butt");
	}

    public virtual void playAnimation(Animations animation)
    {
        playAnimation(m_States[(int)animation]);
    }

    public override void playAnimation(string animationName)
    {
        if (!m_StatesDitctionary.ContainsKey(animationName))
            return;

        if (i_Animator.GetCurrentAnimatorStateInfo(0).IsTag(animationName) && m_Timer > 0.0f ||
            i_Animator.GetCurrentAnimatorStateInfo(0).IsTag(m_States[(int)Animations.Attack]) && m_Timer > 0.0f || 
            i_Animator.GetCurrentAnimatorStateInfo(0).loop && i_Animator.GetCurrentAnimatorStateInfo(0).IsTag(animationName))
            return;

        i_Animator.CrossFade(m_StatesDitctionary[animationName][Random.Range(0, m_StatesDitctionary[animationName].Count)], 0.3f);
        m_Timer = i_Animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
