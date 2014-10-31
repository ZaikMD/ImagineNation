using UnityEngine;
using System.Collections;

public class LoadNextLevel : MonoBehaviour 
{

	AsyncOperation m_Async;

	bool loadNextLevel;

	public MovieTexture m_LoadVideo;

	// Use this for initialization
	void Start ()
	{
		//Application.LoadLevelAsync (Application.loadedLevel + 1);
		m_LoadVideo.Play ();
		m_LoadVideo.loop = true;
		loadNextLevel = true;

	}
	
	// Update is called once per frame
	void Update () 
	{
	//	m_Async = Application.LoadLevelAsync(Application.loadedLevel + 1);
	//	m_Async.allowSceneActivation = false;
    //  m_Text.text = m_Async.progress;
		m_LoadVideo.Play ();
		if(loadNextLevel)
		{
			loadNextLevel = false;
			StartCoroutine(Load());
		}
	}

	IEnumerator waitASecond (float waitTime) 
	{
		yield return new WaitForSeconds (waitTime);
		StartCoroutine(Load ());
	}
	
	IEnumerator Load () 
	{
		switch(GameData.Instance.CurrentLevel)
		{
		case Levels.Level_1:
			switch(GameData.Instance.CurrentSection)
			{
			case Sections.Sections_1:
				m_Async = Application.LoadLevelAsync(Constants.LEVEL1_SECTION1);
				break;

			case Sections.Sections_2:
				m_Async = Application.LoadLevelAsync(Constants.LEVEL1_SECTION2);
				break;

			case Sections.Sections_3:
				m_Async = Application.LoadLevelAsync(Constants.LEVEL1_SECTION3);
				break;
			}

			break;
		}
//		m_Async.allowSceneActivation = false;
		yield return m_Async.isDone;
		yield return SwitchScene();
	}

	bool SwitchScene()
	{
		return Input.GetKeyDown (KeyCode.Space);
		//return true;
	}

}
