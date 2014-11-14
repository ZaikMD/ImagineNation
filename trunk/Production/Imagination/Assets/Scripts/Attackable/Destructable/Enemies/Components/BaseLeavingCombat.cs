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

public abstract class BaseLeavingCombat : MonoBehaviour 
{
    public abstract bool LeaveCombat();
}
