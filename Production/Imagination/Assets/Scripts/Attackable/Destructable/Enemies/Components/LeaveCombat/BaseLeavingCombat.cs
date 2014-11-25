/*
 * Created by Joe Burchill November 14/2014
 * Base Component to determine when the enemy
 * should be leaving combat, used within the
 * Chase Behaviour(s)
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public abstract class BaseLeavingCombat : BaseComponent
{
    public override void start(BaseBehaviour baseBehaviour)
    {

    }

    public abstract bool LeaveCombat(Transform target);
}
