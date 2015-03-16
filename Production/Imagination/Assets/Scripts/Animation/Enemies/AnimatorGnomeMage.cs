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

	protected const string BLANK = "Blank";

	protected override void Start ()
	{
		base.Start ();
		m_States = new string[]{ "Hover",
								 "Clone",
								 "Attack"};
	}

	public virtual void playAnimation(Animations animation)
	{
		playAnimation (m_States [(int)animation]);
	}

	public override void playAnimation(string animationName)
	{
		if(animationName.Equals(m_States[(int)Animations.Attack],System.StringComparison.OrdinalIgnoreCase))
		{

			if(i_Animator.GetCurrentAnimatorStateInfo(1).IsTag(BLANK))
			{
				i_Animator.Play(animationName, 1);
			}
			else
			{
				if(i_Animator.GetCurrentAnimatorStateInfo(2).IsTag(BLANK))
				{
					i_Animator.Play(animationName, 2);
				}
			}
			return;
		}

		i_Animator.Play(animationName);
	}
}