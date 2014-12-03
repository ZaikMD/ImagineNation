// Created by Jason Hein Dec 3, 2014
//
// In progress
//
//
//

#region Changelog

#endregion


using UnityEngine;
using System.Collections;

public class JackInTheBoss : MonoBehaviour
{
	//The current state
	public enum JackInTheBossStates
	{
		PreFight = 0,
		Idle,
		Vulnerable,
		SpringForward,
		Spin,
		Slap,
		TotalStates
	}
	JackInTheBossStates m_State = JackInTheBossStates.PreFight;

	//Attack states of the JackInTheBoss
	JackInTheBossState[] m_AttackStates;


	//Load attack states
	void Start ()
	{
		m_AttackStates = new JackInTheBossState[(int)JackInTheBossStates.TotalStates];
		m_AttackStates[(int)JackInTheBossStates.PreFight] = new PreFight();
		m_AttackStates[(int)JackInTheBossStates.Idle] = new PreFight();
		m_AttackStates[(int)JackInTheBossStates.Vulnerable] = new PreFight();
		m_AttackStates[(int)JackInTheBossStates.SpringForward] = new PreFight();
		m_AttackStates[(int)JackInTheBossStates.Spin] = new PreFight();
		m_AttackStates[(int)JackInTheBossStates.Slap] = new PreFight();
	}
	
	//Update the jack in the boss
	void Update ()
	{
		m_AttackStates[(int)m_State].UpdateState ();
	}

	/// <summary>
	/// Switchs the state of the Boss. It is called by the boss's states in order to switch
	/// </summary>
	public void SwitchState (JackInTheBossStates state)
	{
		m_State = state;
	}
}
