using UnityEngine;
using System.Collections;

public class AnimatorMusicalMortar : AnimatorEnemyBase 
{
	public enum Animations
	{
		Hit,
		Dying,
		Shooting
	};
	
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		
		m_States = new string[]
		{
			"Hit",
			"Dying",
			"Shooting"
		};
	}
	
	public virtual void playAnimation(Animations animation)
	{
		playAnimation (m_States [(int)animation]);
	}
	
	public override void playAnimation(string animationName)
	{
		i_Animator.Play(animationName);
	}
}
