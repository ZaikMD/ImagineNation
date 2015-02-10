//Created by Jason Hein on Feb 10th, 2015

#region Changelog
/*
 * 
 */ 
#endregion


using UnityEngine;
using System.Collections;

//This script calls OnRenderImage to set a custom shader to render the final image
public class MenuBlur : MonoBehaviour
{
	//The material with our shader settings
	public Material m_SettingsRenderMaterial;

	//Material for bluring
	public Material m_BlurMaterial;

	//Render texture of the other camera
	public RenderTexture m_RenderTexture;
	
	//When this camera is about to render
	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		//Render the new screen with a blurred background
		Graphics.Blit (m_RenderTexture, dst, m_BlurMaterial);
		Graphics.Blit (src, dst, m_SettingsRenderMaterial);
	}
}
