//Created by Jason Hein Nov 24, 2014
#region Changelog
/*
 * 
 */ 
#endregion


using UnityEngine;
using System.Collections;

//This script calls OnRenderImage to set a custom shader to render the final image
public class ShaderSettings : MonoBehaviour
{
	//The material with our shader settings
	public Material m_SettingsRenderMaterial;

	//When this camera is about to render
	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		//Render the new screen with our custom shader settings
		Graphics.Blit (src, dst, m_SettingsRenderMaterial);
	}
}
