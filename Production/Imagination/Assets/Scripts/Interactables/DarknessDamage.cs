/*
*DarknessDamage
*
*resposible for killing things slowly
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class DarknessDamage : MonoBehaviour 
{
	//onTriggerEnter damage the destructable if there is one
    void OnTriggerEnter(Collider obj)
    {
        Destructable objDestructable = (Destructable)obj.GetComponentInChildren<Destructable>();
        if (objDestructable != null)
        {
            objDestructable.onHit(new EnemyProjectile());
        }
    }

	//OnTriggerStay damage the destructable if there is one
    void OnTriggerStay(Collider obj)
    {
        Destructable objDestructable = (Destructable)obj.GetComponentInChildren<Destructable>();
        if (objDestructable != null)
        {
            objDestructable.onHit(new EnemyProjectile());
        }
    }
}
