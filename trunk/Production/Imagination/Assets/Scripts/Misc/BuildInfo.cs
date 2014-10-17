/*
*Created by Kristopher Matis and Gregory Fortier
*The script makes a texture that says Alpha Build appear in the bottom left of the screen
*
*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class BuildInfo : MonoBehaviour 
{
	GUITexture m_Texture;

	public Vector2 TextureScale =  new Vector2(1.0f, 1.0f);

	void Awake()
	{
		//makes sure when the next scene loads that the texture doesn't get destroyed
		GameObject.DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		//Gets the texture and sets its position
		m_Texture = gameObject.GetComponent<GUITexture> ();
		m_Texture.pixelInset = new Rect (0, 0, m_Texture.texture.width * TextureScale.x, m_Texture.texture.height * TextureScale.y);
	}
}
