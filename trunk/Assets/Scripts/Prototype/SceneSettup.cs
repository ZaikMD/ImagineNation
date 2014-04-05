using UnityEngine;
using System.Collections;

public class SceneSettup : MonoBehaviour 
{

    public GameObject m_Alex;
    public Camera m_AlexCamera;
    public GameObject m_Derek;
    public Camera m_DerekCamera;
    public GameObject m_Zoey;
    public Camera m_ZoeyCamera;

	// Use this for initialization
	void Start ()
    {
        activatePlayers();   
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void activatePlayers()
    {
        switch (Setting.Instance.m_PlayerOneSelected)
        {
            case Player.Alex:
                {
                    m_Alex.SetActive(true);
                    destroyCamera(m_DerekCamera);
                    destroyCamera(m_ZoeyCamera);
                    Destroy(m_ZoeyCamera);
                    m_Alex.GetComponentInChildren<CameraController>().enabled = true;
                    break;
                }

            case Player.Derek:
                {
                    m_Derek.SetActive(true);
                    destroyCamera(m_AlexCamera);
                    destroyCamera(m_ZoeyCamera);
                    m_Derek.GetComponentInChildren<CameraController>().enabled = true;
                    break;
                }

            case Player.Zoey:
                {
                    m_Zoey.SetActive(true);
                    destroyCamera(m_AlexCamera);
                    destroyCamera(m_ZoeyCamera);
                    m_Zoey.GetComponentInChildren<CameraController>().enabled = true;
                    break;
                }
       
        }


        switch (Setting.Instance.m_PlayerTwoSelected)
        {
            case Player.Alex:
                {
                    m_Alex.SetActive(true);
                    m_Alex.GetComponent<PlayerMovement>().enabled = false;
                    m_AlexCamera.enabled = false;
                    m_Alex.GetComponent<PlayerAIStateMachine>().enabled = true;
                    break;
                }

            case Player.Derek:
                {
                    m_Derek.SetActive(true);
                    m_Derek.GetComponent<PlayerMovement>().enabled = false;
                    m_DerekCamera.enabled = false;
                    m_Derek.GetComponent<PlayerAIStateMachine>().enabled = true;
                    break;
                }

            case Player.Zoey:
                {
                    m_Zoey.SetActive(true);
                    m_Zoey.GetComponent<PlayerMovement>().enabled = false; 
                    m_ZoeyCamera.enabled = true;
                    m_Zoey.GetComponent<PlayerAIStateMachine>().enabled = true;
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
