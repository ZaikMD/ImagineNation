using UnityEngine;
using System.Collections;

public class tempLeoad : MonoBehaviour 
{
	
	AsyncOperation m_Async;
	
	bool m_NextLevelIsLoaded = false;
	
	public MovieTexture m_LoadVideo;
	
	public float MinLoadTime = 2.0f;
	float m_Timer = 0.0f;
	
	// Use this for initialization
	void Start ()
	{
		m_LoadVideo.Play ();
		m_LoadVideo.loop = true;
		
		
		Load ();
		
		m_Timer = MinLoadTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Timer > 0.0f)
		{
			m_Timer -= Time.deltaTime;
			return;
		}
		
		if(m_Async != null && m_Async.isDone)
		{
			if(InputManager.getMenuAcceptDown())
			{
				m_Async.allowSceneActivation = true;
			}
		}
	}
	
	void Load () 
	{
		switch(GameData.Instance.CurrentLevel)
		{
		case Levels.Level_1:
		{
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
				
			case Sections.Sections_Boss:
				m_Async = Application.LoadLevelAsync(Constants.LEVEL1_SECTIONBOSS);
				break;
			}
			break;
		}
		}
		
		m_Async.allowSceneActivation = false;
	}
}
