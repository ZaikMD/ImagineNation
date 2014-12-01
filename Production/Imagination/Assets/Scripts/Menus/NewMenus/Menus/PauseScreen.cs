using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseScreen : MenuV2
{
    static bool m_GameIsPaused = false;
    public static bool IsGamePaused
    {
        get { return m_GameIsPaused; }
    }

    int m_OriginalReadInputFrom;

    bool m_IsSwapping = false;

    CameraSet m_Set1;
    CameraSet m_Set2;

    CameraSet m_MenuCamera;
    CameraSet m_PlayerCameras;

	// Use this for initialization
	protected override void start()
    {
        ShutterCamera[] temp = { m_Camera };
        m_MenuCamera = new CameraSet(temp);

        m_MenuCamera.setCameraEnabled(false);
		m_MenuCamera.setShutterState(true);

        temp = new ShutterCamera[] { TPCamera.Cameras[0], TPCamera.Cameras[1] };
        m_PlayerCameras = new CameraSet(temp);

        m_PlayerCameras.setCameraEnabled(true);
        m_PlayerCameras.setShutterState(false);

        m_Set1 = m_PlayerCameras;
        m_Set2 = m_MenuCamera;
	}
	
	// Update is called once per frame
    protected override void Update() 
    {
        if (!m_IsActiveMenu)
            return;

		if(!m_Camera.IsDoneShutterMove)
			return;

        if (!m_IsSwapping)
        {
            PlayerInput input;
            if (InputManager.getMenuStartDown(m_ReadInputFrom, out input) || (InputManager.getMenuBackDown(m_ReadInputFrom) && m_GameIsPaused))
            {
                m_GameIsPaused = !m_GameIsPaused;

                if (m_GameIsPaused)
                {
                    m_OriginalReadInputFrom = m_ReadInputFrom;
                    m_ReadInputFrom = (int)input;
                }
                else
                {
                     m_ReadInputFrom = m_OriginalReadInputFrom;
                }

                m_IsSwapping = true;
                m_Set1.setShutterState(true);
            }

            if(m_GameIsPaused)
            {
                update();
            }
        }
        else
        {
            if(m_Set1.isSetDoneMove())
            {
                m_Set2.setCameraEnabled(true);
                m_Set1.setCameraEnabled(false);

                m_Set2.setShutterState(false);

                if(m_Set2.isSetDoneMove())
                {
                    m_IsSwapping = false;

                    CameraSet temp = m_Set1;

                    m_Set1 = m_Set2;
                    m_Set2 = temp;
                }
            }
        }
	}
}

class CameraSet
{
    ShutterCamera[] m_Cameras;

    public CameraSet(ShutterCamera[] cameras)
    {
        m_Cameras = cameras;
    }

    public bool isSetDoneMove()
    {
        for (int i = 0; i < m_Cameras.Length; i++)
        {
            if (!m_Cameras[i].IsDoneShutterMove)
                return false;
        }
        return true;
    }

    public void setShutterState(bool isUp)
    {
        for (int i = 0; i < m_Cameras.Length; i++)
        {
			TPCamera temp = m_Cameras[i] as TPCamera;
            m_Cameras[i].ShowShutter = isUp;
        }
    }

    public void setCameraEnabled(bool isEnabled)
    {
        for (int i = 0; i < m_Cameras.Length; i++)
        {
            m_Cameras[i].camera.enabled = isEnabled;
        }
    }
}
