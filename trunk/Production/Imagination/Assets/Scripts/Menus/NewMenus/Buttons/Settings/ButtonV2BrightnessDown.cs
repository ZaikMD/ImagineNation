﻿using UnityEngine;
using System.Collections;

public class ButtonV2BrightnessDown: ButtonV2 
{
	public Slider i_Slider;

	protected override void start ()
	{
		base.start ();
		i_Slider.MaxValue = Constants.BRIGHTNESS_MAX;
		i_Slider.MinValue = Constants.BRIGHTNESS_MIN;
		i_Slider.Value = GameData.Instance.Brightness;
	}

	public override void use ()
	{
		GameData.Instance.Brightness = Mathf.Clamp(GameData.Instance.Brightness - Constants.BRIGHTNESS_INCREMENT,
		                                           Constants.BRIGHTNESS_MIN,
		                                           Constants.BRIGHTNESS_MAX);
		i_Slider.Value = GameData.Instance.Brightness;
	}
}
