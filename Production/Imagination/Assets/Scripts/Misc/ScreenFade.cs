﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	public float m_FadeInSpeed;
	public float m_FadeOutSpeed;
	public bool m_FadeIn = false;

	public Image m_ImageForFade;

	private bool m_FadeOut = false;

	private float m_CurrentFadeTime;

	// Use this for initialization
	void Start ()
	{
		m_ImageForFade = GetComponent<Image>();

		if(m_FadeIn)
		{
			m_ImageForFade.color = Color.black;
			BeginFadeIn();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
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

				Application.LoadLevel(Constants.MAIN_MENU_NAME);
			}
		}
	}

	void FadeIn()
	{
		m_ImageForFade.color = Color.Lerp(m_ImageForFade.color, Color.clear, m_FadeInSpeed * Time.deltaTime);	
	}

	void FadeOut()
	{
		m_ImageForFade.color = Color.Lerp(m_ImageForFade.color, Color.black, m_FadeOutSpeed * Time.deltaTime);	
	}

	public void BeginFadeIn()
	{
		m_ImageForFade.color = new Vector4(1, 1, 1, 0.95f);
		m_FadeIn = true;
	}

	public void BeginFadeOut()
	{
		m_ImageForFade.color = new Vector4(1, 1, 1, 0.05f);
		m_FadeOut = true;
	}
}
