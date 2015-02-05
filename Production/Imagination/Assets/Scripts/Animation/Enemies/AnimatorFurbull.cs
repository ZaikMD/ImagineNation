using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorFurbull : AnimatorEnemyBase 
{
	// Use this for initialization
	protected override void Start () 
    {
        base.Start();

        m_States.Add("Idle", new List<string>());

        m_States["Idle"].Add("Idle");
        m_States["Idle"].Add("HeadTurn");
        m_States["Idle"].Add("Breathing");
        m_States["Idle"].Add("FeetStamp");

        m_States.Add("Run", new List<string>());

        m_States["Run"].Add("Run Start");

        m_States.Add("Attack", new List<string>());

        m_States["Attack"].Add("Head Butt");
	}

}
