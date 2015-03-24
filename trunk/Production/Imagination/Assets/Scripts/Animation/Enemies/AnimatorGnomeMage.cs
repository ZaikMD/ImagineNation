using UnityEngine;
using System.Collections;

public class AnimatorGnomeMage : AnimatorEnemyBase
{
	public enum Animations
	{
		Hover,
		Clone,
		Attack
	};

	protected string[] m_Attacks = new string[] {"Attack Left Arm", "Attack Right Arm"};

	protected override void Start ()
	{
		base.Start ();
		m_States = new string[]{ "Idle",
								 "Clone",
								 "Attack"};
	}

	public virtual void playAnimation(Animations animation)
	{
		playAnimation (m_States [(int)animation]);
	}

	public override void playAnimation(string animationName)
	{
		if (i_Animator.GetCurrentAnimatorStateInfo (1).IsTag (animationName))
			return;

		if(animationName.Equals(m_States[(int)Animations.Attack],System.StringComparison.OrdinalIgnoreCase))
		{
			i_Animator.Play(m_Attacks[Random.Range(0, m_Attacks.Length)]);
			return;
		}

		i_Animator.Play(animationName);
	}
}