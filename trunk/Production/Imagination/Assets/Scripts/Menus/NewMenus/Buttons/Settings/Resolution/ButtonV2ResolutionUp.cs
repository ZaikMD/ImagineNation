using UnityEngine;
using System.Collections;

public class ButtonV2ResolutionUp : ButtonV2Resolution
{
    public override void use()
    {
        m_CurrentResolution = Mathf.Clamp(m_CurrentResolution + 1, 0, Screen.resolutions.Length - 1);
        setResolution();
    }
}
