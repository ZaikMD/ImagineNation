/*

TO USE:

Do nothing. It is instantiated by the camera script.



Created by Jason "The Casual" Hein on 3/31/2014

*/


using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour
{
	//Widht and height of the Reticle
	public const float RETICLE_DISTANCE = 50.0f;
	const float RETICAL_SCREEN_SIZE = 30.0f;

	//Reticle Position on screen
	Vector2 m_ReticleScreenPosition = Vector2.zero;

	//Reticle texture
	Texture2D m_ReticleTexture;
	Texture2D m_ReticleTextureRed;
	bool m_OnSomething = false;
	bool m_CanDraw = false;
	

	/// <summary>
	/// Sets the crosshairs texture.
	/// </summary>
	public void Load()
	{
		//Hide cursor
		//Screen.showCursor = false;

		//Sets the texture of the Reticle
		m_ReticleTexture = (Texture2D)Resources.Load("CrossHair_NormalState");
		m_ReticleTextureRed = (Texture2D)Resources.Load("CrossHair_HighlitedState");

		//Sets the initial screen position to the center of the screen
		m_ReticleScreenPosition = new Vector2((Screen.width - RETICAL_SCREEN_SIZE) / 2, (Screen.height - RETICAL_SCREEN_SIZE) /2);

	}

	/// <summary>
	/// Returns the position of the Reticle in world space.
	/// </summary>
	public Vector3 getTargetPosition()
	{
		return transform.position;
	}

	/// <summary>
	/// Sets if the reticle can be drawn
	/// </summary>
	public void canDraw(bool canDraw)
	{
		m_CanDraw = canDraw;
	}

	/// <summary>
	/// Sets where to draw the reticle on screen.
	/// </summary>
	public void setReticleScreenPosition (Vector2 screenPosition)
	{
		m_ReticleScreenPosition = screenPosition;
	}

	/// <summary>
	/// Sets the reticle's position in world space.
	/// </summary>
	/// <param name="position">Position.</param>
	public void setReticlePosition (Vector3 position)
	{
		transform.position = position;
	}

	/// <summary>
	/// Sets reticle to red or blue.
	/// </summary>
	/// <param name="isOnSomething">If set to <c>true</c> is on something.</param>
	public void SetIsOnSomething(bool isOnSomething)
	{
		m_OnSomething = isOnSomething;
	}

	//Draws the Reticle
	void OnGUI()
	{
		if (!m_CanDraw)
		{
			return;
		}
		if (!m_OnSomething && m_ReticleTexture != null)
		{
			GUI.DrawTexture(new Rect(m_ReticleScreenPosition.x - RETICAL_SCREEN_SIZE / 2.0f, m_ReticleScreenPosition.y - RETICAL_SCREEN_SIZE / 2.0f, RETICAL_SCREEN_SIZE, RETICAL_SCREEN_SIZE), m_ReticleTexture);
		}
		else if (m_ReticleTextureRed != null)
		{
			GUI.DrawTexture(new Rect(m_ReticleScreenPosition.x - RETICAL_SCREEN_SIZE / 2.0f, m_ReticleScreenPosition.y - RETICAL_SCREEN_SIZE / 2.0f, RETICAL_SCREEN_SIZE, RETICAL_SCREEN_SIZE), m_ReticleTextureRed);
		}
	}
}
