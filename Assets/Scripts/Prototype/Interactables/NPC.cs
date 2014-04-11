/*
TO USE:


Attach script to NPC.

Add trigger collider to object.

Set the text to draw, and the number of lines to break the text into.






Created by Jason "The Casual" Hein




4/9/2014
	Now delays text and punctuation.
	Added chat bubble.
*/



using UnityEngine;
using System.Collections;

public class NPC : InteractableBaseClass {

	//Text to show
	public string m_Text = " ";
	public int m_Lines = 1;
	string m_ShownText = "";

	//Is interacted with
	bool m_ShowText = false;

	//Letter timer
	float m_LetterTimer = 0.0f;
	const float DEFAULT_SPEECH_SPEED = 0.04f;
	const float PERIOD_SPEECH_DELAY = 0.7f;
	const float END_SPEECH_DELAY = 2.0f;

	//Variables for designers
	public Vector2 m_NormalizedTextPos = new Vector2(0.45f, 0.5f);
	Rect m_Rectangle;
	Rect m_ChatRect;

	//Font size
	public float m_FontSize = 20.0f;

	//Player to renable movmeent
	GameObject m_Player = null;

	//Index of letter
	int m_Index = 0;

	//Texture
	Texture2D m_SpeechBubble;



	// Initialization
	void Start ()
	{
		m_IsExitable = false;
		m_Type = InteractableType.NPC;

		m_SpeechBubble = (Texture2D)Resources.Load("ChatBox");

		//Where to draw the text
		m_Rectangle = new Rect (m_NormalizedTextPos.x * Screen.width - m_FontSize, m_NormalizedTextPos.y * Screen.height - m_FontSize,
		                        m_FontSize * m_Text.Length / m_Lines * 0.55f, m_FontSize * 1.5f * m_Lines);
		m_ChatRect = new Rect (m_Rectangle.x - m_FontSize / 2.0f, m_Rectangle.y - m_FontSize / 2.0f, m_Rectangle.width + m_FontSize / 2.0f, m_Rectangle.height + m_FontSize / 2.0f);
	}

	// On tick
	void Update ()
	{
		//Add text to talk bubble
		if (m_ShowText)
		{
			addToTalk();
		}
	}

	// Show text
	public void setShowText(bool show, GameObject obj)
	{
		m_Player = obj;
		m_ShowText = show;

		//dialog begin/end
		if (m_ShowText)
		{
			sendEvent(ObeserverEvents.DialogueBegin);
		}
		else
		{
			sendEvent(ObeserverEvents.DialogueEnd);
		}
	}

	//Adds the next letter to the speech
	void addToTalk()
	{
		if (m_LetterTimer <= 0.0f && m_Index < m_Text.Length)
		{
			//Delay at sentence pauses
			if (m_Text[m_Index] == '.' || m_Text[m_Index] == ',' || m_Text[m_Index] == '!' || m_Text[m_Index] == '?')
			{
				m_LetterTimer = PERIOD_SPEECH_DELAY;
			}
			else if (m_Index == m_Text.Length)
			{
				m_LetterTimer = END_SPEECH_DELAY;
			}
			else
			{
				m_LetterTimer = DEFAULT_SPEECH_SPEED;
			}

			//Add text
			m_ShownText += m_Text[m_Index];
			m_Index++;
		}
		else if (m_LetterTimer > 0.0f)
		{
			m_LetterTimer -= Time.deltaTime;
		}
		else
		{
			m_Player.GetComponent<PlayerState>().exitInteracting();
			setShowText(false, null);
			
			m_ShownText = " ";
			m_Index = 0;
		}
	}

	// In range
	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}
	}

	// Out of range
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}
	}

	// Draw label
	void OnGUI()
	{
		if (m_ShowText)
		{
			//Add font size and color

			GUI.DrawTexture(m_ChatRect, m_SpeechBubble);
			string text = "<color=black><size=" + m_FontSize + ">" + m_ShownText + "</size></color>";
			GUI.Label(m_Rectangle, text);
		}
	}


}
