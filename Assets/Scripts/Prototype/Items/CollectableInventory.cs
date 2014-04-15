/*
TO USE:

Attach this script to singletons gameobject.




Created by Jason "The Casual" Hein

*/



using UnityEngine;
using System.Collections;

public class CollectableInventory : MonoBehaviour {
	
	//Collectables carried
	int m_Collectables = 0;

	//Draw Timer
	float m_Timer = 0.0f;
	const float DRAW_TIMER = 2.5f;
	bool m_Draw = false;

	//Only one instance of the inventory
	public static CollectableInventory Instance{ get; private set; }

	//Texture
	Texture2D m_Icon;
	Rect m_WhereToDraw;
	Rect m_TextRect;
	const int FONT_SIZE = 20;


	// On Start
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy it... there can be only one
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}
		
		//set the instance
		Instance = this;

		m_Icon = (Texture2D)Resources.Load("LightBrightCounter");
		m_WhereToDraw = new Rect (Screen.width * 0.80f, Screen.height * 0.1f, 120, 80);
		m_TextRect = new Rect (Screen.width * 0.876f, Screen.height * 0.107f, 100, 60);
		
		//prevents this object being destroyed between scene loads
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_Timer > 0.0f)
		{
			m_Timer -= Time.deltaTime;

			if (m_Timer <= 0.0f)
			{
				m_Draw = false;
			}
		}
	}

	// Pick Up Collectable
	public void collect()
	{
		m_Collectables++;
		m_Draw = true;
		m_Timer = DRAW_TIMER;
	}

	// Draw HUD
	void OnGUI()
	{
		if (m_Draw)
		{
			GUI.DrawTexture(m_WhereToDraw, m_Icon);
			string text = "<color=white><size=" + FONT_SIZE + ">" + m_Collectables.ToString() + "</size></color>";
			GUI.Label(m_TextRect , text);
		}
	}
}
