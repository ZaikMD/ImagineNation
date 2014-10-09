using UnityEngine;
using System.Collections;
/// <summary>
/// Created by Zach Dubuc
/// 
/// An interface for anything that can be attacked to inherit
/// </summary>

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented- Zach Dubuc
*
* 
*/
public interface Attackable
{
     void onHit(PlayerProjectile proj); //Two overloaded function so whatever inherits from Attackable can choose what they can be attacked by, Players or Enemies

     void onHit(EnemyProjectile proj);
}
