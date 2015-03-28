using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*	created: Kole Tackney, Mar, 27, 2015
 * 
 *  this script creates a fade to black and the end of 
 * 	scenes and a fade in at the beginning
 * 
 */





public class ScreenFade : MonoBehaviour {


	//Controls the speed of fade
	public float m_FadeInSpeed;
	public float m_FadeOutSpeed;

	//the scene we load after fade out
	public Sections m_SectionTooLoad;

	//This is the image that will appear
	public Image m_ImageForFade;

	//are we fading
	public bool m_FadeIn = false;
	private bool m_FadeOut = false;

	// Use this for initialization
	void Start ()
	{
		//Get references
		m_ImageForFade = GetComponent<Image>();

		//begin fading i if we should
		if(m_FadeIn)
		{
			//set our image to black
			m_ImageForFade.color = Color.black;
			BeginFadeIn();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Check if we should fade in
		if(m_FadeIn)
		{
			FadeIn();

			if(m_ImageForFade.color.a <= 0.05f)
			{
				m_FadeIn = false;
				m_ImageForFade.color = Color.clear;
			}
		}
		else if(m_FadeOut)
		{
			FadeOut();
			
			if(m_ImageForFade.color.a >= 0.95f)
			{
				m_FadeOut = false;
				m_ImageForFade.color = Color.black;

				LoadNextLevel();
			}
		}
	}

	//Fade in 
	void FadeIn()
	{
		m_ImageForFade.color = Color.Lerp(m_ImageForFade.color, Color.clear, m_FadeInSpeed * Time.deltaTime);	
	}

	//Fade out
	void FadeOut()
	{
		m_ImageForFade.color = Color.Lerp(m_ImageForFade.color, Color.black, m_FadeOutSpeed * Time.deltaTime);	
	}

	//Begin to fade in
	public void BeginFadeIn()
	{
		m_ImageForFade.color = new Vector4(1, 1, 1, 0.95f);
		m_FadeIn = true;
	}

	//Begin to fade out
	public void BeginFadeOut()
	{
		m_ImageForFade.color = new Vector4(1, 1, 1, 0.05f);
		m_FadeOut = true;
	}

	//Loads a level based on passed in value
	public void LoadNextLevel()
	{
		switch (m_SectionTooLoad) 
		{
			case Sections.Sections_1:
			Application.LoadLevel(Constants.LEVEL1_SECTION1);
			break;

			case Sections.Sections_2:
			Application.LoadLevel(Constants.LEVEL1_SECTION2);
			break;
		
			case Sections.Sections_3:
			Application.LoadLevel(Constants.LEVEL1_SECTION3);
			break;

			case Sections.Sections_Boss:
			Application.LoadLevel(Constants.LEVEL1_SECTIONBOSS);
			break;

			default:
			Application.LoadLevel(Constants.MAIN_MENU_NAME);
			break;
		}
	}
}
