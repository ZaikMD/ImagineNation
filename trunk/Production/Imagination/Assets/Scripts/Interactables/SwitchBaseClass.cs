using UnityEngine;
using System.Collections;

public class SwitchBaseClass : MonoBehaviour
{
    bool m_Activated;

    public virtual bool getActive()
    {
        return m_Activated;
    }
}
