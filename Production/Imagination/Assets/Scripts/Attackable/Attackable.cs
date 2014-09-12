using UnityEngine;
using System.Collections;

public interface Attackable
{
     void OnHit(PlayerProjectile proj); //Two overloaded function so whatever inherits from Attackable can choose what they can be attacked by, Players or Enemies

     void OnHit(EnemyProjectile proj);
}
