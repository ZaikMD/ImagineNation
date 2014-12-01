using UnityEngine;
using System.Collections;

public class SpinTopCombatMovement : BaseMovement 
{
    public override Vector3 Movement(GameObject target)
    {
        return target.transform.position;
    }
}
