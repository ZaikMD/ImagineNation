using UnityEngine;
using System.Collections;

public class ActionAreaDetector : MonoBehaviour 
{
    public ActionArea m_CurrentActionArea { get; private set; }

    public void enteredActionArea(ActionArea actionArea)
    {
        if (actionArea != null)
        {
            m_CurrentActionArea = actionArea;
        }
    }

    public void exitedActionArea(ActionArea actionArea)
    {
        if (actionArea == m_CurrentActionArea)
        {
            m_CurrentActionArea = null;
        }
    }
}
