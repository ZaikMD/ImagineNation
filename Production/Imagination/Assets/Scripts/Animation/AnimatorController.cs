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
            else
#endif
            {
                
            }
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

    public void setFloat(string name, float value)
    {
        i_Animator.SetFloat(name, value);
    }

    public void setFloat(int name, float value)
    {
        i_Animator.SetFloat(name, value);
    }

    public void setInt(string name, int value)
    {
        i_Animator.SetInteger(name, value);
    }

    public void setInt(int name, int value)
    {
        i_Animator.SetInteger(name, value);
    }

    public void setBool(string name, bool value)
    {
        i_Animator.SetBool(name, value);
    }

    public void setBool(int name, bool value)
    {
        i_Animator.SetBool(name, value);
    }
}
