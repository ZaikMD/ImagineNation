using UnityEngine;
using System.Collections;

public class LoadNextLevel : MonoBehaviour {

	AsyncOperation m_Async;

	public TextMesh m_Text;
	float m_Progress;
	bool m_CanLoadNextLevel;
	bool m_LoadingLevel;

	// Use this for initialization
	void Start ()
	{
		Application.LoadLevel (Application.loadedLevel + 1);
//		m_LoadingLevel = false;
//		m_CanLoadNextLevel = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	//	m_Async = Application.LoadLevelAsync(Application.loadedLevel + 1);
	//	m_Async.allowSceneActivation = false;
//		m_Text.text = m_Async.progress;
		if(!m_LoadingLevel)
		{
			StartCoroutine(waitAsecond (1));
		}

		if(m_Async != null)
		{
			m_Progress = m_Async.progress * 100;
		}
		else
		{
			m_Progress = 0;
		}
		Debug.Log (m_Progress);
		m_Text.text = m_Progress.ToString();

		if(m_CanLoadNextLevel)
		{
			m_Text.text = "Press start";

			if(InputManager.getMenuStart())
			{

				m_Async.allowSceneActivation = true;
			}
		}
	}

	IEnumerator waitAsecond (float waitTime) 
	{
		yield return new WaitForSeconds (waitTime);
		StartCoroutine(Load ());
	}
	
	IEnumerator Load () 
	{
		Debug.Log ("there");
		m_Async = Application.LoadLevelAsync(Application.loadedLevel + 1);
		m_Async.allowSceneActivation = false;
		m_LoadingLevel = true;
		yield return m_Async.isDone;
		yield return m_Progress = 100;
		m_CanLoadNextLevel = true;
	}

	void SwitchScene()
	{
		if(m_Async != null)
		{
			m_Async.allowSceneActivation = true;
		}
	}

}
