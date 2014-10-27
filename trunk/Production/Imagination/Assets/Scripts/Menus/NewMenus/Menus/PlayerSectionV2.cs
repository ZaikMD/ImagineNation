﻿/*
 * Created by: Kris MAtis
 * data organizer class for multi player character selection
 * 
 */

#region ChangeLog
/*
 * 27/10/2014 edit: script made
 */
#endregion

using UnityEngine;
using System.Collections;

public class PlayerSectionV2
{
    GameObject m_Model;
    Transform m_MountPoint;
    Transform m_OriginalMountpoint;

    Characters m_Character;

    bool m_IsMounted = false;
    public bool IsMounted
    {
        get { return m_IsMounted; }
        set { m_IsMounted = value; }
    }

    bool m_IsScaling = false;
    public bool IsScaling
    {
        get { return m_IsScaling; }
        set { m_IsScaling = value; }
    }

    const float SCALE_AMOUNT = 3.0f;
    const float LERP_AMOUNT = 0.15f;

    public PlayerSectionV2( GameObject model, Transform mountPoint, Characters character)
    {
        m_Model = model;
        m_MountPoint = mountPoint;
        m_Character = character;
    }

    public virtual void update()
    {
        if (m_IsScaling)
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
        m_MountPoint = m_OriginalMountpoint;
        m_IsMounted = false;
        m_IsScaling = false;
    }
}
