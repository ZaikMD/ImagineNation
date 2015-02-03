using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour 
{
    [SerializeField]
    protected Animator i_Animator = null;

    protected string[] m_States;

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
}
