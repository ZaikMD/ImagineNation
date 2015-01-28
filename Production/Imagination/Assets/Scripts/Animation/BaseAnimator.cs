using UnityEngine;
using System.Collections;

public class BaseAnimator : MonoBehaviour 
{
    public Animation m_Animation = null;

    //*** the enum that will be used for keys must start at zero and not skip any values ****
    public AnimationClip[] m_AnimationClips = null;

    protected string m_CurrentClip = "";

    protected const float DEFAULT_FADE = 0.1f;
    protected const float DEFAULT_WEIGHT = 0.5f;

    public bool isPlaying
    {
        get
        {
            return m_Animation.isPlaying;
        }
    }


	// Use this for initialization
    protected virtual void Start() 
    {
        if (m_Animation == null)
        {
            m_Animation = gameObject.GetComponentInChildren<Animation>();
#if UNITY_EDITOR || DEBUG
            if (m_Animation == null)
            {
                Debug.LogError("You fucked up there should be an \"Animation\" component somewhere on this object or its children");
            }
#endif
        }

        setUp();
	}

    //======================================================================================

    public virtual void playAnimation(int animationNumber, float fadeLength = DEFAULT_FADE)
    {
        playAnimation(m_AnimationClips[animationNumber].name, fadeLength);
    }

    public virtual void playAnimation(string animationName, float fadeLength = DEFAULT_FADE)
    {
        if (m_CurrentClip.CompareTo(animationName) != 0)
        {
            m_CurrentClip = animationName;
            m_Animation.CrossFade(animationName, fadeLength);
        }
    }


    //======================================================================================
    //blend

    public virtual void addAnimation(int animationNumber, float targetWeight = DEFAULT_WEIGHT, float fadeLength = DEFAULT_FADE)
    {
        addAnimation(m_AnimationClips[animationNumber].name, targetWeight, fadeLength);
    }

    public virtual void addAnimation(string animationName, float targetWeight = DEFAULT_WEIGHT, float fadeLength = DEFAULT_FADE)
    {
        m_Animation.Blend(animationName, targetWeight, fadeLength);
    }

    //======================================================================================

    protected virtual void setUp()
    {
        for (int i = 0; i < m_AnimationClips.Length; i++)
        {
            m_Animation.AddClip(m_AnimationClips[i], m_AnimationClips[i].name);
        }
    }
}
