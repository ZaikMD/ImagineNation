using UnityEngine;
using System.Collections;

public class CutSceneManager : MonoBehaviour , Observer
{

	public Camera[] SceneCameras;
	public Camera MainCamera;

	bool m_InScene;

	float cutSceneTimer;

	public Subject m_PartOne;

	public enum scene
	{
		sceneOne,
		SceneTwo,
		SceneThree
	}

	public static CutSceneManager Instance{ get; private set; }
	/// <summary>
	/// Setting the instance.
	/// </summary>
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy this... there can be only one
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}
		
		//set the instance
		Instance = this;
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(gameObject);
	



		foreach(Camera cam in SceneCameras)
		{
			cam.enabled = false;
		}
	
	}

	// Use this for initialization
	void Start ()
	{
		foreach(Camera cam in SceneCameras)
		{
			cam.enabled = false;
		}

		m_PartOne.addObserver (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_InScene)
		{
			cutSceneTimer -= Time.deltaTime;
			if(cutSceneTimer <= 0)
			{
				m_InScene = false;
				foreach(Camera cam in SceneCameras)
				{
					cam.enabled = false;
				}
				MainCamera.enabled = true;
			}
		}

	}

	void play(scene scene)
	{
		switch(scene)
		{
			case scene.sceneOne:
			{
				SceneOne();
				break;
			}
			case scene.SceneTwo:
			{
				ScreneTwo();
				break;
			}
			case scene.SceneThree:
			{
				ScreneThree();
				break;
			}
		}
	}

	void SceneOne()
	{
		cutSceneTimer = 5;
		MainCamera.enabled = false;
		m_InScene = true;
		SceneCameras [0].enabled = true;
	}

	void ScreneTwo()
	{
		cutSceneTimer = 5;
		MainCamera.enabled = false;
		m_InScene = true;
		SceneCameras [1].enabled = true;
	}

	void ScreneThree()
	{
		cutSceneTimer = 5;
		MainCamera.enabled = false;
		m_InScene = true;
		SceneCameras [2].enabled = true;
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.Used && sender == m_PartOne)
		{
			play(scene.sceneOne);
		}
		/*
		if(sender == m_PartOne)
		{
			play(scene.sceneOne);
		}
		*/
	}


}
