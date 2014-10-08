using UnityEngine;
using System.Collections;

/*
 * 
 * 
 * 
 * 10/8/2014, Kole Tackney
 * Added Animation Constants 
 * 
 */

public static class Constants
{
	public const string PLAYER_STRING = "Player";
	public const string ALEX_STRING = "Alex";
	public const string DEREK_STRING = "Derek";
	public const string ZOE_STRING = "Zoe";

	public const string PLAYER_PROJECTILE_STRING = "PlayerProjectile";
	public const string ENEMY_PROJECTILE_STRING = "EnemyProjectile";

	public const string CHECK_POINT_1_STRING = "CheckPoint_1";
	public const string CHECK_POINT_2_STRING = "CheckPoint_2";
	public const string CHECK_POINT_3_STRING = "CheckPoint_3";

	public const string MAIN_CAMERA_STRING = "MainCamera";

    public const string WALL_TAG_STRING = "Wall";
    public const string MOVING_PLATFORM_TAG_STRING = "MovingPlatform";

	public static class Animations
	{
		public const string IDLE = "Idle";
		public const string WALK = "Walk";
		public const string RUN = "Run";
		public const string JUMP = "Jump";
		public const string LANDING = "Landing";
		public const string FALLING = "Falling";
		public const string DEATH = "Death";
		public const string DOUBLE_SLASH = "DoubleSlash";
		public const string RIGHT_HOOK = "RightHook";
		public const string OVERHEAD_SLASH = "OverHeadSlash";
		public const string LOOK_AROUND = "LookAround";
		public const string TAKING_WEAPON_OUT = "TakingWeaponOut";
		public const string GLIDING = "Gliding";
	}

	public static class Sounds
	{
		 
		public const string	JUMP = ("Sounds/Alex/Alex_Jump");
		public const string WALK = ("Sounds/Common/Jump_Pad");
		public const string RUN = ("Sounds/Common/footsteps_carpet_edit2");
		public const string WEAPON_WOOSH = ("Sounds/Common/Woosh");
		public const string COLLECTABLE =("Sounds/Common/Collectable");
		public const string JUMPAD =("Sounds/Common/Jump_Pad");
		public const string ZIPPER = ("Sounds/Common/Zipper");

		//Alex Sounds
		public const string ALEX_FIRST_WEAPON_HIT = ("Sounds/Alex/First_Weapon_hit_Alex");
		public const string ALEX_SECOND_WEAPON_HIT = ("Sounds/Alex/Second_Weapon_Hit_Alex");
		public const string ALEX_THIRD_WEAPON_HIT = ("Sounds/Alex/Final_Weapon_hit_Alex");
		public const string ALEX_HURT =  ("Sounds/Alex/Alex_Painful_Grunt");
		public const string ALEX_JUMP =  ("Sounds/Alex/Alex_Jump");
		public const string ALEX_DEATH = ("Sounds/Alex/Alex_Death");

		//Derek Sounds
		public const string DEREK_FIRST_WEAPON_HIT  =  ("Sounds/Derek/Derek_First_Hit");
		public const string DEREK_SECOND_WEAPON_HIT =  ("Sounds/Derek/Derek_Second_Hit");
		public const string DEREK_THIRD_WEAPON_HIT =  ("Sounds/Derek/Derek_Third_Hit");
		public const string DEREK_HURT =  ("Sounds/Common/Derek_Painful_Grunt");
		public const string DEREK_JUMP = ("Sounds/Derek/Derek_Jump");
		public const string DEREK_DEATH = ("Sound/Derek/Derek_Death");

		//Zoey Sounds
		public const string ZOEY_FIRST_WEAPON_HIT = ("Sounds/Zoey/Zoey_First_Hit");
		public const string ZOEY_SECOND_WEAPON_HIT =  ("Sounds/Zoey/Zoey_Second_Hit");
		public const string ZOEY_THIRD_WEAPON_HIT =  ("Sounds/Zoey/Zoey_Third_Hit");
		public const string ZOEY_HURT =  ("Sounds/Zoey/Zoey_Painful_Grunt");
		public const string ZOEY_DEATH =  ("Sounds/Zoey/Zoey_Death");
		public const string ZOEY_JUMP =  ("Sounds/Zoey/Zoey_Jump");
		public const string ZOEY_WINGS_OPEN = ("Sounds/Zoey/Wings_Open");
		public const string ZOEY_WINGS_CLOSE =  ("Sounds/Zoey/Wings_Close");
		public const string ZOEY_WINGS_DEPLOY =  ("Sounds/Zoey/Wings_Deploy");

	}


}
