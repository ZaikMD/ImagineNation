using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public MovieTexture m_Video;
    bool hasPlayed = false;

    void Start()
    {
        m_Video.Play();
    }
	// Use this for initialization
	void OnLoad() 
	{
		m_Video.Play();
        hasPlayed = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
            if (!m_Video.isPlaying)
            {
                Application.LoadLevel(Application.loadedLevel + 1);
            }
    }
}
