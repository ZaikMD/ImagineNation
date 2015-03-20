using UnityEngine;
using System.Collections;

public class LoadNextLevel : MonoBehaviour 
{
	
	AsyncOperation m_Async = null;
	
	public float MinLoadTime = 2.0f;
	float m_Timer = 0.0f;

	bool m_IsDoneLoading = false;


	public Texture i_Instructions = null;
	public Texture i_InstructionsBackground = null;
	Rect m_InstructionsRect;

	public Texture i_LoadingTexture = null;
	public Texture i_PrompTexture = null;
	public Vector2 i_TextureDrawPos = new Vector2(0.75f, 0.25f);
	public Vector2 i_TextureDrawSize = new Vector2(0.25f, 0.25f);

	// Use this for initialization
	void Start ()
	{		
		StartCoroutine( "Load");
		
		m_Timer = MinLoadTime;

		m_InstructionsRect = new Rect (0.0f, 0.0f, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Timer > 0.0f)
		{
			m_Timer -= Time.deltaTime;
			return;
		}
		
		if(m_IsDoneLoading)
		{
			if(InputManager.getMenuAcceptDown())
			{
				m_Async.allowSceneActivation = true;
			}
		}
	}

	void OnGUI() 
	{
		GUI.DrawTexture (m_InstructionsRect, i_InstructionsBackground, ScaleMode.StretchToFill);
		GUI.DrawTexture (m_InstructionsRect, i_Instructions, ScaleMode.ScaleToFit);

		if (!m_IsDoneLoading || m_Timer > 0.0f) 
		{
			GUI.DrawTexture( new Rect(Screen.width  * i_TextureDrawPos.x, 
	                          		  Screen.height * i_TextureDrawPos.y, 
			                          Screen.width  * i_TextureDrawSize.x, 
			                          Screen.height * i_TextureDrawSize.y), i_LoadingTexture, ScaleMode.ScaleToFit);
			return;
		}
		GUI.DrawTexture( new Rect(Screen.width  * i_TextureDrawPos.x, 
		                          Screen.height * i_TextureDrawPos.y, 
		                          Screen.width  * i_TextureDrawSize.x, 
		                          Screen.height * i_TextureDrawSize.y), i_PrompTexture, ScaleMode.ScaleToFit);
	}

	
	IEnumerator Load () 
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
			default:
#if UNITY_EDITOR || DEBUG
				Debug.Log("Failed to find scene");
#endif
				break;
			}
			break;
		}
		default:
#if UNITY_EDITOR || DEBUG
			Debug.Log("Failed to find scene");
#endif
			break;
		}

		m_Async.priority = int.MaxValue;
		m_Async.allowSceneActivation = false;

		do
		{
			yield return null;
		}while(m_Async.progress > 0.999f);

		m_IsDoneLoading = true;
		yield return null;
	}
}
