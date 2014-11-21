using UnityEngine;
using System.Collections;

public class ProximityAggro : BaseEnterCombat 
{
    public float AggroRange = 20.0f;
    BaseTargeting m_Targeting;

    public override bool EnterCombat()
    {
        if (Vector3.Distance(transform.position, m_Targeting.CurrentTarget().transform.position) < AggroRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
