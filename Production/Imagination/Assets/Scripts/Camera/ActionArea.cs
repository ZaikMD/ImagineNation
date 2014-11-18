using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionArea : MonoBehaviour 
{
    List<ActionAreaDetector> m_AcionAreaDetectorsDetected = new List<ActionAreaDetector>();

    public GameObject CameraMountPoint;

    void OnTriggerEnter(Collider obj)
    {
        GameObject parentObject = obj.gameObject;

        while (parentObject.transform.parent != null)
        {
            parentObject = parentObject.transform.parent.gameObject;
        }

        ActionAreaDetector detected = parentObject.GetComponentInChildren<ActionAreaDetector>();
        if (detected != null)
        {
            detected.enteredActionArea(this);
            m_AcionAreaDetectorsDetected.Add(detected);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        GameObject parentObject = obj.gameObject;

        while (parentObject.transform.parent != null)
        {
            parentObject = parentObject.transform.parent.gameObject;
        }

        ActionAreaDetector detected = parentObject.GetComponentInChildren<ActionAreaDetector>();

        if (detected != null)
        {
            for(int i = 0; i < m_AcionAreaDetectorsDetected.Count; i++)
            {
                if (detected == m_AcionAreaDetectorsDetected[i])
                {
                    detected.exitedActionArea(this);
                    m_AcionAreaDetectorsDetected.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
