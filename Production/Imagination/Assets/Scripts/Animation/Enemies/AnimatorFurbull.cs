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
}
