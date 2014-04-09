using UnityEngine;
using System.Collections;

public class NPC : InteractableBaseClass {

	//Text to show
	public string m_Text = " ";

	//Is interacted with
	bool m_ShowText = false;

	//Timer for how long to keep text on screen
	float m_Timer = 0.0f;
	public float m_TimerLength = 5.0f;

	//Variables for designers
	public Vector2 m_NormalizedTextPos = new Vector2(0.0f,0.0f);
	Rect m_Rectangle;

	//Font size
	public float m_FontSize = 20.0f;



	// Initialization
	void Start ()
	{
		m_IsExitable = false;
		m_Type = InteractableType.NPC;

		//Where to draw the text
		m_Rectangle = new Rect (m_NormalizedTextPos.x * Screen.width - m_FontSize, m_NormalizedTextPos.y * Screen.height - m_FontSize,
		                        m_NormalizedTextPos.x * Screen.width + m_FontSize, m_NormalizedTextPos.y * Screen.height + m_FontSize);

		//Add text size and color
		m_Text = "<color=black><size=" + m_FontSize + ">" + m_Text + "</size></color>";
	}

	// On tick
	void Update ()
	{
		if (m_Timer > 0.0f)
		{
			m_Timer -= Time.deltaTime;

			if (m_Timer <= 0.0f)
			{
				setShowText(false);
			}
		}
	}

	// Show text
	public void setShowText(bool show)
	{
		m_ShowText = show;
		if (m_ShowText == true)
		{
			m_Timer = m_TimerLength;


			//GREG PLAY SOUNG HERE <-------------------------------------------------------------

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
			GUI.Label(m_Rectangle, m_Text);
		}
	}


}
