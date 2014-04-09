﻿using UnityEngine;
using System.Collections;

public class SceneSettup : MonoBehaviour 
{

    public GameObject m_Alex;
    public Camera m_AlexCamera;
    public GameObject m_Derek;
    public Camera m_DerekCamera;
    public GameObject m_Zoey;
    public Camera m_ZoeyCamera;


	Player m_PlayerOne;
	Player m_PlayerTwo;

	void Awake()
	{
		m_PlayerOne = (Player)PlayerPrefs.GetInt ("CurrentPlayerOne");
		m_PlayerTwo = (Player)PlayerPrefs.GetInt ("CurrentPlayerTwo");
		activatePlayers ();
	}

    void activatePlayers()
    {
        switch (m_PlayerOne)
        {

            case Player.Alex:
                {
                    m_Alex.SetActive(true);
					m_Alex.GetComponent<AlexPlayerState>().m_IsActive = true;
					m_Alex.GetComponent<PlayerAIStateMachine>().m_IsActive = false;
					m_Derek.GetComponent<PlayerMovement>().m_CameraTransform = m_AlexCamera.transform;
					m_Zoey.GetComponent<PlayerMovement>().m_CameraTransform = m_AlexCamera.transform;
					destroyCamera(m_DerekCamera);
					destroyCamera(m_ZoeyCamera);
					m_Alex.GetComponentInChildren<CameraController>().enabled = true;
                    break;
                }

            case Player.Derek:
                {
                    m_Derek.SetActive(true);
					m_Derek.GetComponent<DerekPlayerState>().m_IsActive = true;
					m_Derek.GetComponent<PlayerAIStateMachine>().m_IsActive = false;
                	destroyCamera(m_AlexCamera);
					destroyCamera(m_ZoeyCamera);
					
					m_Alex.GetComponent<PlayerMovement>().m_CameraTransform = m_DerekCamera.transform;
					m_Zoey.GetComponent<PlayerMovement>().m_CameraTransform = m_DerekCamera.transform;

			        m_Derek.GetComponentInChildren<CameraController>().enabled = true;
                    break;
                }

            case Player.Zoey:
                {
                    m_Zoey.SetActive(true);
					m_Zoey.GetComponent<ZoeyPlayerState>().m_IsActive = true;
					m_Zoey.GetComponent<PlayerAIStateMachine>().m_IsActive = false;
                   	destroyCamera(m_DerekCamera);
					destroyCamera(m_AlexCamera);
					
					m_Derek.GetComponent<PlayerMovement>().m_CameraTransform = m_ZoeyCamera.transform;
					m_Alex.GetComponent<PlayerMovement>().m_CameraTransform = m_ZoeyCamera.transform;
					
                    m_Zoey.GetComponentInChildren<CameraController>().enabled = true;
                    break;
                }
       
        }


        switch (m_PlayerTwo)
        {
            case Player.Alex:
                {
                    m_Alex.SetActive(true);
					if(m_PlayerOne == Player.Zoey)
					{
						m_Derek.SetActive(false);
					}
					if(m_PlayerOne == Player.Derek)
					{
						m_Zoey.SetActive(false);
					}	
						
                    m_Alex.GetComponent<AlexPlayerState>().m_IsActive = false;
                    m_Alex.GetComponent<PlayerAIStateMachine>().m_IsActive = true;
                   // m_AlexCamera.enabled = false;
					break;
                }

            case Player.Derek:
                {
					if(m_PlayerOne == Player.Zoey)
					{
						m_Alex.SetActive(false);
					}
					if(m_PlayerOne == Player.Alex)
					{
						m_Zoey.SetActive(false);
					}	

                    m_Derek.SetActive(true);
                    m_Derek.GetComponent<DerekPlayerState>().m_IsActive = false;
                    m_DerekCamera.enabled = false;
                    m_Derek.GetComponent<PlayerAIStateMachine>().m_IsActive = true;
                    break;
                }

            case Player.Zoey:
                {
					if(m_PlayerOne == Player.Alex)
					{
						m_Derek.SetActive(false);
					}
					if(m_PlayerOne == Player.Derek)
					{
						m_Alex.SetActive(false);
					}	

                    m_Zoey.SetActive(true);
                    m_Zoey.GetComponent<ZoeyPlayerState>().m_IsActive = false; 
                    m_ZoeyCamera.enabled = false;
                    m_Zoey.GetComponent<PlayerAIStateMachine>().m_IsActive = true;
                    break;
                }

        }
    }


    void destroyCamera(Camera camera )
    { 
        Destroy(camera.GetComponent<GUILayer>());
        Destroy(camera.GetComponent<CameraController>());
        Destroy(camera);
    }




}
