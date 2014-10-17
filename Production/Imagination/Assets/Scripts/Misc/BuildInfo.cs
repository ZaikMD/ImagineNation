using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class BuildInfo : MonoBehaviour 
{
	GUITexture m_Texture;

	public Vector2 TextureScale =  new Vector2(1.0f, 1.0f);

	void Awake()
	{
		GameObject.DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		m_Texture = gameObject.GetComponent<GUITexture> ();

		m_Texture.pixelInset = new Rect (0, 0, m_Texture.texture.width * TextureScale.x, m_Texture.texture.height * TextureScale.y);
	}
}
