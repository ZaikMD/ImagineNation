using UnityEngine;
using System.Collections;

public class ButtonV2CameraRotationSpeed : ButtonV2
{

	public Slider i_Slider;
	
	protected float m_Increment = Constants.CAMERA_ROTATE_SPEED_INCREMENT;
	
	protected override void start ()
	{
		base.start ();
		i_Slider.MaxValue = Constants.CAMERA_ROTATE_SPEED_MAX;
		i_Slider.MinValue = Constants.CAMERA_ROTATE_SPEED_MIN;
		i_Slider.Value = Constants.CAMERA_ROTATE_SPEED_DEFAULT;
	}

	public override void use ()
	{
		GameData.Instance.CameraRotationScaleModifyer = Mathf.Clamp(GameData.Instance.CameraRotationScaleModifyer + m_Increment,
		                                                            Constants.CAMERA_ROTATE_SPEED_MIN, 
		                                                            Constants.CAMERA_ROTATE_SPEED_MAX);		
		i_Slider.Value = GameData.Instance.CameraRotationScaleModifyer;
	}
}
