using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneSettup : MonoBehaviour 
{

    public GameObject m_Alex;
    public Camera m_AlexCamera;
    public GameObject m_Derek;
    public Camera m_DerekCamera;
    public GameObject m_Zoey;
    public Camera m_ZoeyCamera;
	List<GameObject> m_Checkpoints;

	Player m_PlayerOne;
	Player m_PlayerTwo;

    Level m_StartingCheckpoint;

	GameObject m_StartPoint;

	void Awake()
	{
        Debug.Log("testing");
        getCheckPoint(); 
		setStage ();
		activatePlayers ();
                   
        CheckpointManager.m_Instance.m_CurrentCheckPoint = m_StartPoint.GetComponent<Checkpoint> ();
	}

    void activatePlayers()
    {
        
        m_PlayerOne = (Player)PlayerPrefs.GetInt ("CurrentPlayerOne");
		m_PlayerTwo = (Player)PlayerPrefs.GetInt ("CurrentPlayerTwo");
        
        switch (m_PlayerOne)
        {

            case Player.Alex:
                {
                    m_Alex.SetActive(true);
					m_Alex.GetComponent<AlexPlayerState>().m_IsActive = true;
					m_Alex.GetComponent<PlayerAIStateMachine>().m_IsActive = false;
					m_Derek.GetComponent<PlayerMovement>().m_CameraTransform = m_AlexCamera.transform;
					m_Zoey.GetComponent<PlayerMovement>().m_CameraTransform = m_AlexCamera.transform;
					m_Derek.GetComponent<PlayerState>().m_CameraController = m_AlexCamera.GetComponent<CameraController>();
					m_Zoey.GetComponent<PlayerState>().m_CameraController = m_AlexCamera.GetComponent<CameraController>();
					destroyCamera(m_DerekCamera);
					destroyCamera(m_ZoeyCamera);
					m_Alex.GetComponentInChildren<CameraController>().enabled = true;
					m_Alex.GetComponent<NavMeshAgent>().enabled = false;
			//		CutSceneManager.Instance.MainCamera = m_AlexCamera;
					RespawnManager.Instance.PlayerOne = m_Alex;
				
		//TODO			m_Alex.transform.position = m_Checkpoint.transform.position;
			m_Alex.transform.position = m_StartPoint.transform.position + new Vector3(0, 3, 0);;
			
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
			m_Alex.GetComponent<PlayerState>().m_CameraController = m_DerekCamera.GetComponent<CameraController>();
			m_Zoey.GetComponent<PlayerState>().m_CameraController = m_DerekCamera.GetComponent<CameraController>();
					m_Derek.GetComponentInChildren<CameraController>().enabled = true;
            		RespawnManager.Instance.PlayerOne = m_Derek;     
					m_Derek.GetComponent<NavMeshAgent>().enabled = false;
		//			CutSceneManager.Instance.MainCamera = m_DerekCamera;


			//TODO			m_Derek.transform.position = m_Checkpoint.transform.position;
			m_Derek.transform.position = m_StartPoint.transform.position+ new Vector3(0, 3, 0);;
			
			break;
		}

            case Player.Zoey:
                {
                    m_Zoey.SetActive(true);
					m_Zoey.GetComponent<ZoeyPlayerState>().m_IsActive = true;
					m_Zoey.GetComponent<PlayerAIStateMachine>().m_IsActive = false;
                   	destroyCamera(m_DerekCamera);
					destroyCamera(m_AlexCamera);
				
				//Camera stuff
					m_Derek.GetComponent<PlayerMovement>().m_CameraTransform = m_ZoeyCamera.transform;
					m_Alex.GetComponent<PlayerMovement>().m_CameraTransform = m_ZoeyCamera.transform;
					m_Alex.GetComponent<PlayerState>().m_CameraController = m_ZoeyCamera.GetComponent<CameraController>();
					m_Derek.GetComponent<PlayerState>().m_CameraController = m_ZoeyCamera.GetComponent<CameraController>();
					m_Zoey.GetComponentInChildren<CameraController>().enabled = true;
            		
			        RespawnManager.Instance.PlayerOne = m_Zoey;        
			m_Zoey.GetComponent<NavMeshAgent>().enabled = false;
			//		CutSceneManager.Instance.MainCamera = m_ZoeyCamera;
			//TODO			m_Zoey.transform.position = m_Checkpoint.transform.position;

                    m_Zoey.transform.position = m_StartPoint.transform.position + new Vector3(0, 3, 0);
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
						Destroy(m_Derek);
						destroyCamera(m_DerekCamera);
						m_Zoey.GetComponent<PlayerAIStateMachine>().m_Partner = m_Alex;
						m_Alex.GetComponent<PlayerAIStateMachine>().m_Partner = m_Zoey;
					CharacterSwitch.Instance.removeObserver(m_Derek.GetComponent<PlayerState>());
				//m_Derek.SetActive(false);
					}
					if(m_PlayerOne == Player.Derek)
					{
						Destroy(m_Zoey);
						destroyCamera(m_ZoeyCamera);
						m_Derek.GetComponent<PlayerAIStateMachine>().m_Partner = m_Alex;
						m_Alex.GetComponent<PlayerAIStateMachine>().m_Partner = m_Derek;
						CharacterSwitch.Instance.removeObserver(m_Zoey.GetComponent<PlayerState>());
						//m_Zoey.SetActive(false);
					}	
						
                    m_Alex.GetComponent<AlexPlayerState>().m_IsActive = false;
                    m_Alex.GetComponent<PlayerAIStateMachine>().m_IsActive = true;
				///	m_Alex.GetComponent<PlayerAIStateMachine>().m_Partner = 
					destroyCamera(m_AlexCamera);
			    	RespawnManager.Instance.PlayerTwo = m_Alex;
			
			//	Vector3 position = new Vector3(m_Checkpoint.transform.position.x - 5, m_Checkpoint.transform.position.y, m_Checkpoint.transform.position.z);
			//TODO			m_Alex.transform.position = position;
                    m_Alex.transform.position = m_StartPoint.transform.position + new Vector3(2.0f, 0.0f, 0.0f);

					m_Alex.GetComponent<NavMeshAgent>().enabled = true;
				
					break;
                }

            case Player.Derek:
                {
					if(m_PlayerOne == Player.Zoey)
					{
						Destroy(m_Alex);
						destroyCamera(m_AlexCamera);
						m_Derek.GetComponent<PlayerAIStateMachine>().m_Partner = m_Zoey;
						m_Zoey.GetComponent<PlayerAIStateMachine>().m_Partner = m_Derek;
						CharacterSwitch.Instance.removeObserver(m_Alex.GetComponent<PlayerState>());
						//m_Alex.SetActive(false);
					}
					if(m_PlayerOne == Player.Alex)
					{
						//m_Zoey.SetActive(false);
						Destroy(m_Zoey);
						destroyCamera(m_ZoeyCamera);
						m_Derek.GetComponent<PlayerAIStateMachine>().m_Partner = m_Alex;
						m_Alex.GetComponent<PlayerAIStateMachine>().m_Partner = m_Derek;
						m_Zoey.SetActive(false);
						CharacterSwitch.Instance.removeObserver(m_Zoey.GetComponent<PlayerState>());
					}	

                    m_Derek.SetActive(true);
                    m_Derek.GetComponent<DerekPlayerState>().m_IsActive = false;
                    m_DerekCamera.enabled = false;
					destroyCamera(m_DerekCamera);
                    m_Derek.GetComponent<PlayerAIStateMachine>().m_IsActive = true;
					RespawnManager.Instance.PlayerTwo = m_Derek;

			//	Vector3 position = new Vector3(m_Checkpoint.transform.position.x - 5, m_Checkpoint.transform.position.y, m_Checkpoint.transform.position.z);
			//TODO		m_Derek.transform.position = position;
                    m_Derek.transform.position = m_StartPoint.transform.position + new Vector3(2.0f, 0.0f, 0.0f);

					m_Derek.GetComponent<NavMeshAgent>().enabled = true;

                    break;
                }

            case Player.Zoey:
                {
					if(m_PlayerOne == Player.Alex)
					{
						Destroy(m_Derek);
						destroyCamera(m_DerekCamera);
						m_Zoey.GetComponent<PlayerAIStateMachine>().m_Partner = m_Alex;
						m_Alex.GetComponent<PlayerAIStateMachine>().m_Partner = m_Zoey;
						CharacterSwitch.Instance.removeObserver(m_Derek.GetComponent<PlayerState>());
						m_Derek.SetActive(false);
					}
					if(m_PlayerOne == Player.Derek)
					{
						m_Derek.GetComponent<PlayerAIStateMachine>().m_Partner = m_Zoey;
						m_Zoey.GetComponent<PlayerAIStateMachine>().m_Partner = m_Derek;
						destroyCamera(m_AlexCamera);
						Destroy(m_Alex);
						CharacterSwitch.Instance.removeObserver(m_Alex.GetComponent<PlayerState>());
						m_Alex.SetActive(false);
					}	

                    m_Zoey.SetActive(true);
                    m_Zoey.GetComponent<ZoeyPlayerState>().m_IsActive = false; 
                    m_ZoeyCamera.enabled = false;
					destroyCamera(m_ZoeyCamera);
                    m_Zoey.GetComponent<PlayerAIStateMachine>().m_IsActive = true;
					RespawnManager.Instance.PlayerTwo = m_Zoey;		

			//	Vector3 position = new Vector3(m_Checkpoint.transform.position.x - 5, m_Checkpoint.transform.position.y, m_Checkpoint.transform.position.z);
			//TODO		m_Derek.transform.position = position;
                    m_Zoey.transform.position = m_StartPoint.transform.position + new Vector3(2.0f, 0.0f, 0.0f);
					
					m_Zoey.GetComponent<NavMeshAgent>().enabled = true;
					
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

    void movePlayersToCheckpoint()
    { 
        
    
    }
    
    void getCheckPoint()
    {
        m_StartingCheckpoint = (Level)PlayerPrefs.GetInt("CurrentLevel");
        
        switch (m_StartingCheckpoint)
        {

            case Level.LevelOneStart:
                {
                    m_StartPoint = GameObject.Find("StartPoint");
                    Debug.Log("startPoint");
                    break;
                }
            case Level.LevelOnePartTwo:
                {
                    m_StartPoint = GameObject.Find("CheckPoint1");
                    Debug.Log("checkpoint1");
                    break;
                }
            case Level.LevelOnePartThree:
                {
                    m_StartPoint = GameObject.Find("CheckPoint2");
                    Debug.Log("checkpoint2");
                    break;
                }
            case Level.LevelOnePartFour:
                {
                    m_StartPoint = GameObject.Find("CheckPoint3");
                    Debug.Log("checkpoint3");
                    break;
                }
            case Level.LevelOnePartFive:
                {
                    m_StartPoint = GameObject.Find("CheckPoint4");
                    Debug.Log("checkpoint4");
                    break;
                }
            case Level.LevelOnePartSix:
                {
                    m_StartPoint = GameObject.Find("CheckPoint5");
                    Debug.Log("checkpoint5");
                    break;
                }
            case Level.LevelOnePartSeven:
                {
                    m_StartPoint = GameObject.Find("CheckPoint6");
                    Debug.Log("checkpoint6");
                    break;
                }
        
       }  
    }


	void setStage ()
	{
		Stage currentStage = (Stage)PlayerPrefs.GetInt ("CurrentLevelStage");

		switch(currentStage)
		{
			case Stage.StartStage:
			{
				GameManager.Instance.m_CurrentStage = Stage.StartStage;
				GameManager.Instance.setUpSenders (2);
			break;
			}
			case Stage.StageOne:
			{
				GameManager.Instance.m_CurrentStage = Stage.StageOne;
				GameManager.Instance.setUpSenders (2);
				break;
			}
			case Stage.StageTwo:
			{
				GameManager.Instance.m_CurrentStage = Stage.StageTwo;
				GameManager.Instance.setUpSenders (1);
				break;
			}
			case Stage.StageThree:
			{
				GameManager.Instance.m_CurrentStage = Stage.StageThree;
				GameManager.Instance.setUpSenders (1);
				break;
			}

			case Stage.StageFour:
			{
				GameManager.Instance.m_CurrentStage = Stage.StageFour;
			//GameManager.Instance.setUpSenders ();
				break;
			}

		}

		GameManager.Instance.levelState ();

	}

}
