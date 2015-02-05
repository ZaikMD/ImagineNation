using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorEnemyBase : AnimatorController 
{
    protected Dictionary<string, List<string>> m_States = new Dictionary<string, List<string>>();

    float m_Timer = 0.0f;

    public override void playAnimation(string animationName)
    {
        if (!m_States.ContainsKey(animationName))
            return;

        if (i_Animator.GetCurrentAnimatorStateInfo(0).IsTag(animationName) && m_Timer > 0.0f)
            return;

        i_Animator.Play(m_States[animationName][Random.Range(0, m_States[animationName].Count)]);
        m_Timer = i_Animator.GetCurrentAnimatorStateInfo(0).length;
    }

    protected void Update()
    {
        if(m_Timer > 0.0f)
            m_Timer -= Time.deltaTime;
    }
}
