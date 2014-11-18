using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovieTexture))]

public class OpeningSplashScreen : MonoBehaviour {

	float m_UserAspect;

	public MovieTexture m_Video;

	// Use this for initialization
	void Start () 
	{
	  m_Video.Play();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//video is no longer playing
		if(!m_Video.isPlaying)
		{
			//video is over, set aspect ratio back to players choice, and load menu scene
			Application.LoadLevel(1);
		}	
	}
}
