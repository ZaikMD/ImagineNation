using UnityEngine;
using System.Collections;

/*
 * 
 * 
 * 
 * 10/8/2014, Kole Tackney
 * Added Animation Constants 
 * Added Sound Constants
 * 
 *  23/10/2014 kris Matis : added the playerinput[] constant (had to do work arounds)
 * 
 */

public static class Constants
{
	static PlayerInput[] m_PLAYER_INPUT_ARRAY;// = new PlayerInput[(int)PlayerInput.Count] { PlayerInput.Keyboard, PlayerInput.GamePadOne, PlayerInput.GamePadTwo, PlayerInput.GamePadThree, PlayerInput.GamePadFour, PlayerInput.All };
	public static PlayerInput[] PLAYER_INPUT_ARRAY
	{
		get
		{
			if(m_PLAYER_INPUT_ARRAY == null)
			{
				m_PLAYER_INPUT_ARRAY = new PlayerInput[(int)PlayerInput.Count] { PlayerInput.Keyboard, 
                                                                                 PlayerInput.GamePadOne, 
                                                                                 PlayerInput.GamePadTwo, 
                                                                                 PlayerInput.GamePadThree, 
                                                                                 PlayerInput.GamePadFour, 
                                                                                 PlayerInput.All };
			}
			return m_PLAYER_INPUT_ARRAY;
		}
	}

	public const string PLAYER_STRING = "Player";
	public const string ALEX_STRING = "Alex";
	public const string DEREK_STRING = "Derek";
	public const string ZOE_STRING = "Zoe";
	public const string ALEX_WITH_MOVEMENT_STRING = "Alex With Movement";
	public const string DEREK_WITH_MOVEMENT_STRING = "Derek With Movement";
	public const string ZOE_WITH_MOVEMENT_STRING = "Zoe With Movement";


	public const string PLAYER_PROJECTILE_STRING = "PlayerProjectile";
	public const string ENEMY_PROJECTILE_STRING = "EnemyProjectile";

	public const string CHECK_POINT_1_STRING = "CheckPoint_1";
	public const string CHECK_POINT_2_STRING = "CheckPoint_2";
	public const string CHECK_POINT_3_STRING = "CheckPoint_3";

	public const string MAIN_CAMERA_STRING = "MainCamera";
	public const string SOUND_MANAGER = "SoundManager";
    public const string COLLECTABLE_MANAGER = "CollectableManager";

    public const string WALL_TAG_STRING = "Wall";
    public const string MOVING_PLATFORM_TAG_STRING = "MovingPlatform";
	public const string MOVING_BLOCK_TAG_STRING = "MovingBlock";

	public const string LOADING_SCREEN = "LoadingScreen";
	public const string LEVEL1_SECTION1 = "Level1_Section1";
	public const string LEVEL1_SECTION2 = "Level1_Section2";
	public const string LEVEL1_SECTION3 = "Level1_Section3";

	public struct Animations
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

	public struct Sounds
	{
		 
		public const string	JUMP = "Sounds/Alex/Alex_Jump";
		public const string WALK = "Sounds/Common/footsteps_carpet_walk";
		public const string RUN = "Sounds/Common/footsteps_carpet_edit3";
		public const string WEAPON_WOOSH = "Sounds/Common/Woosh";
		public const string COLLECTABLE = "Sounds/Common/Collectable";
		public const string JUMPAD = "Sounds/Common/Jump_Pad";
		public const string ZIPPER = "Sounds/Common/Zipper";
		public const string GATE_OPEN = "Sounds/Interactable/Gate_Down";

		//Alex Sounds
		public const string ALEX_FIRST_WEAPON_HIT = "Sounds/Alex/First_Weapon_hit_Alex";
		public const string ALEX_SECOND_WEAPON_HIT = "Sounds/Alex/Second_Weapon_Hit_Alex";
		public const string ALEX_THIRD_WEAPON_HIT = "Sounds/Alex/Final_Weapon_hit_Alex";
		public const string ALEX_HURT = "Sounds/Alex/Alex_Painful_Grunt";
		public const string ALEX_JUMP = "Sounds/Alex/Alex_Jump";
		public const string ALEX_DEATH = "Sounds/Alex/Alex_Death";

		//Derek Sounds
		public const string DEREK_FIRST_WEAPON_HIT  =  "Sounds/Derek/Derek_First_Hit";
		public const string DEREK_SECOND_WEAPON_HIT =  "Sounds/Derek/Derek_Second_Hit";
		public const string DEREK_THIRD_WEAPON_HIT =  "Sounds/Derek/Derek_Third_Hit";
		public const string DEREK_HURT =  "Sounds/Common/Derek_Painful_Grunt";
		public const string DEREK_JUMP = "Sounds/Derek/Derek_Jump";
		public const string DEREK_DEATH = "Sound/Derek/Derek_Death";

		//Zoey Sounds
		public const string ZOEY_FIRST_WEAPON_HIT = "Sounds/Zoey/Zoey_First_Hit";
		public const string ZOEY_SECOND_WEAPON_HIT = "Sounds/Zoey/Zoey_Second_Hit";
		public const string ZOEY_THIRD_WEAPON_HIT =  "Sounds/Zoey/Zoey_Third_Hit";
		public const string ZOEY_HURT =  "Sounds/Zoey/Zoey_Painful_Grunt";
		public const string ZOEY_DEATH =  "Sounds/Zoey/Zoey_Death";
		public const string ZOEY_JUMP =  "Sounds/Zoey/Zoey_Jump";
		public const string ZOEY_WINGS_OPEN = "Sounds/Zoey/Wings_Open";
		public const string ZOEY_WINGS_CLOSE =  "Sounds/Zoey/Wings_Close";
		public const string ZOEY_WINGS_DEPLOY =  "Sounds/Zoey/Wings_Deploy";

		//Enemy sounds
		public const string MAGE_SHOOT =  "Sounds/Enemies/Gnome_Mage_Spell_Cast";
		public const string MAGE_HIT =  "Sounds/Enemies/Gnome_Mage_Spell_Hit";

	}


}
