using UnityEngine;
using System.Collections;

public class LoadNextLevel : MonoBehaviour {

	AsyncOperation m_Async;

	// Use this for initialization
	void Start ()
	{
		m_Async = Application.LoadLevelAsync(Application.loadedLevel + 1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log(m_Async.isDone);
		if(m_Async.isDone)
		{
			Application.LoadLevel(Application.loadedLevel +1);
		}
	}
}
