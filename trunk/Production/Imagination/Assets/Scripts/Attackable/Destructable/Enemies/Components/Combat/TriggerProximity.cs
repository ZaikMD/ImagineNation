using UnityEngine;
using System.Collections;

public class TriggerProximity : BaseCombat 
{
    public override void Combat()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Constants.PLAYER_STRING)
        {

        }
    }
}
