/*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 27/10/2014 Edit: no longer a stub
*
* 27/10/2014 edit: made the upate fully comented
 * 
*/
#endregion

using UnityEngine;
using System.Collections;

public class InputSelectV2 
{
    //the point that the model is mounted to
    Transform m_MountPoint;
    public Transform MountPoint
    {
        get { return m_MountPoint; }
        set 
        { 
            m_MountPoint = value;
            m_IsMounted = true;
            m_Scale = true;
        }
    }
    Transform m_OriginalMountPoint;

    //the model to show the user whats happening
    GameObject m_Model;
    public GameObject Model
    {
        get { return m_Model; }
    }

    //the input that corresponds to the input select
    PlayerInput m_InputType;
    public PlayerInput InputType
    {
        get { return m_InputType; }
    }

    //how fast the class lerps
    const float LERP_AMOUNT = 0.15f;
    
    //how much the model will scale
    const float SCALE_AMOUNT = 3.0f;
    
    //is the model scaling or not
    bool m_Scale = false;
    public bool Scale
    {
        get { return m_Scale; }
        set { m_Scale = value; }
    }

    //is the model mounted?
    bool m_IsMounted = false;
    public bool IsMounted
    {
        get { return m_IsMounted; }
        set { m_IsMounted = value; }
    }

    //is the input set to ready?
    bool m_IsReady = false;
    public bool IsReady
    {
        get { return m_IsReady; }
        set { m_IsReady = value; }
    }

    //constructor to initialize the class
    public InputSelectV2(Transform mountPoint, GameObject model, PlayerInput inputType)
    {
        m_MountPoint = mountPoint;
        m_OriginalMountPoint = mountPoint;
        m_Model = model;
        m_InputType = inputType;
    }

    public virtual void update()
    {
        if (m_Scale)
        {//scale up
            m_Model.transform.localScale = Vector3.Lerp(m_Model.transform.localScale, new Vector3(SCALE_AMOUNT, SCALE_AMOUNT, SCALE_AMOUNT), LERP_AMOUNT);
        }
        else
        {//scale down
            m_Model.transform.localScale = Vector3.Lerp(m_Model.transform.localScale, new Vector3(1.0f, 1.0f, 1.0f), LERP_AMOUNT);
        }
        //lerp the model to its mount point
        m_Model.transform.position = Vector3.Lerp(m_Model.transform.position, m_MountPoint.position, LERP_AMOUNT);
    }

    public void resetMountPoint()
    {
        //mini reset 
        m_MountPoint = m_OriginalMountPoint;
        m_IsMounted = false;
        m_Scale = false;
    }

    public void fullReset()
    {
        //reset everything
        resetMountPoint();
        m_IsReady = false;
    }
}
