using UnityEngine;
using System.Collections;

public class CutSceneManager : MonoBehaviour {

	public Camera[] SceneCameras;


	public enum scene
	{

	}
	void Awake()
	{
		foreach(Camera cam in SceneCameras)
		{
			cam.enabled = false;
		}
	
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void play(int scene)
	{
		switch(scene)
		{
			case 1:
			{
				SceneOne();
				break;
			}
		}
	}

	void SceneOne()
	{


	}

}
