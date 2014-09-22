using UnityEngine;
using System.Collections;

public class DarknessDamage : MonoBehaviour 
{
    void OnTriggerEnter(Collider obj)
    {
        Destructable objDestructable = (Destructable)obj.GetComponentInChildren<Destructable>();
        if (objDestructable != null)
        {
            objDestructable.onHit(new EnemyProjectile());
        }
    }

    void OnTriggerStay(Collider obj)
    {
        Destructable objDestructable = (Destructable)obj.GetComponentInChildren<Destructable>();
        if (objDestructable != null)
        {
            objDestructable.onHit(new EnemyProjectile());
        }
    }
}
