using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public MovieTexture m_Video;

	// Use this for initialization
	void Start () 
	{
		m_Video.Play();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_Video.isPlaying)
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
