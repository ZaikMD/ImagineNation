using UnityEngine;
using System.Collections;

public class ButtonV2Brightness : ButtonV2 
{
	public Slider i_Slider;

	protected float m_Increment = Constants.BRIGHTNESS_INCREMENT;

	protected override void start ()
	{
		base.start ();
		i_Slider.MaxValue = Constants.BRIGHTNESS_MAX;
		i_Slider.MinValue = Constants.BRIGHTNESS_MIN;
		i_Slider.Value = GameData.Instance.Brightness;
	}
	
	public override void use (PlayerInput usedBy = PlayerInput.None)
	{
		GameData.Instance.Brightness = Mathf.Clamp(GameData.Instance.Brightness + m_Increment,
		                                           Constants.BRIGHTNESS_MIN,
		                                           Constants.BRIGHTNESS_MAX);
		i_Slider.Value = GameData.Instance.Brightness;
	}
}
