using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {


	public MovieTexture mov;
	// Use this for initialization
	void Start () 
	{
		renderer.material.mainTexture = mov;
		mov.Play ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mov.isPlaying == false)
		{
			Application.LoadLevel("Menu");
		}
	}


	void playVideo()
	{
//		renderer.material.mainTexture.Play ();
	}
}
