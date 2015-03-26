using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class HudNumbers : MonoBehaviour 
{
	public Texture[] i_NumberTextures;
	public ScaleMode i_ScaleMode = ScaleMode.ScaleToFit;

	public string num = "500";

	Regex m_Validate = new Regex("^\\d*$");

	void OnGUI()
	{
		drawNumber (num, new Rect (Screen.width * 0.25f, Screen.height * 0.25f,
	                           		 Screen.width * 0.5f, Screen.height * 0.5f));
	}

	/// <summary>
	/// Draws the number.
	/// cannot do negatives
	/// </summary>
	/// <param name="number">Number.</param>
	/// <param name="Size">Size.</param>
	/// <param name="scaleNumbers">If set to <c>true</c> scale numbers.</param>
	public void drawNumber(string number, Rect Size, bool scaleNumbers = true)
	{
		if (!(number.Length > 0))
			return;

		if (!m_Validate.IsMatch (number)) 
		{
#if DEBUG || UNITY_EDITOR
			Debug.LogError("Invalid String");
#endif
			return;
		}

		if(scaleNumbers)
		{
			Size.width = Size.width / (float)number.Length;

			draw(number, Size);
		}
		else
		{
			draw(number, Size);
		}
	}

	void draw(string number, Rect Size)
	{
		GUI.DrawTexture(Size, i_NumberTextures[int.Parse(number.Substring(0, 1))], i_ScaleMode);
		
		Size.position = new Vector2(Size.position.x + Size.width, Size.position.y);

		drawNumber(number.Substring(1, number.Length - 1), Size, false);
	}
}
