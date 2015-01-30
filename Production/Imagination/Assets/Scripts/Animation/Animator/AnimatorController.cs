using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour 
{
    public Animator i_Animator = null;

    protected string[] m_States = new string[]{"test","test2"};

    protected virtual void Start()
    {
        if (i_Animator == null)
        {
            i_Animator = gameObject.GetComponentInChildren<Animator>();
#if UNITY_EDITOR || DEBUG
            if (i_Animator == null)
            {
                Debug.LogError("You fucked up there should be an \"Animator\" component somewhere on this object or its children");
            }
#endif
        }
    }

    //======================================================================================

    public virtual void playAnimation(int animationNumber)
    {
        playAnimation(m_States[animationNumber]);
    }

    public virtual void playAnimation(string animationName)
    {
        i_Animator.Play(animationName);
    }


    //======================================================================================
    //blend
    /*
    public virtual void addAnimation(int animationNumber, float targetWeight = DEFAULT_WEIGHT, float fadeLength = DEFAULT_FADE)
    {
        addAnimation(m_AnimationClips[animationNumber].name, targetWeight, fadeLength);
    }

    public virtual void addAnimation(string animationName, float targetWeight = DEFAULT_WEIGHT, float fadeLength = DEFAULT_FADE)
    {
        m_Animation.Blend(animationName, targetWeight, fadeLength);
    }*/
}
