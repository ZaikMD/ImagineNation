using UnityEngine;
using System.Collections;

public abstract class ButtonV2Resolution : ButtonV2 
{
    protected int m_CurrentResolution = -1;

    protected override void start()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if(Screen.currentResolution.width == Screen.resolutions[i].width && Screen.currentResolution.height == Screen.resolutions[i].height)
            {
                m_CurrentResolution = i;
                break;
            }
        }
    }

    protected void setResolution()
    {
        Screen.SetResolution(Screen.resolutions[m_CurrentResolution].width, Screen.resolutions[m_CurrentResolution].height, true);
    }
}
