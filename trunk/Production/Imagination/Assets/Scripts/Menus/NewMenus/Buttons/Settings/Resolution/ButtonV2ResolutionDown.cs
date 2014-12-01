using UnityEngine;
using System.Collections;

public class ButtonV2ResolutionDown : ButtonV2Resolution 
{
	public override void use(PlayerInput usedBy = PlayerInput.None)
    {
        m_CurrentResolution = Mathf.Clamp(m_CurrentResolution - 1, 0, Screen.resolutions.Length - 1);
        setResolution();
    }
}
