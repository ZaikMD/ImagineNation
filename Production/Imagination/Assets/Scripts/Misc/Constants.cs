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
                                                                                 PlayerInput.GamePadFour};
			}
			return m_PLAYER_INPUT_ARRAY;
		}
	}
	public const int LIVE_INITIAL_COUNT = 3;
	public const int LIVES_MAX = 99;
	public const short LIGHT_PEGS_NEEDED_TO_GAIN_LIVES = 40;
	public const string KILLZONE_STRING = "Kill Zone";

	public const string BRIGHTNESS_PROPERTY_NAME = "_Brightness";
	public const float BRIGHTNESS_INCREMENT = 0.1f;
	public const float BRIGHTNESS_MIN = 0.1f;
	public const float BRIGHTNESS_MAX = 2.0f;
	public const float BRIGHTNESS_DEFAULT = 1.0f;

	public const float CAMERA_ROTATE_SPEED_MIN = 0.5f;
	public const float CAMERA_ROTATE_SPEED_MAX = 2.0f;
	public const float CAMERA_ROTATE_SPEED_DEFAULT = 1.0f;
	public const float CAMERA_ROTATE_SPEED_INCREMENT = 0.1f;

    //This will control how long the hud is displayed for
    public const float HUD_ON_SCREEN_TIME = 3.0f;

	public const string IGNORE_RAYCAST = "Ignore Raycast";

	public const string PLAYER_STRING = "Player";
	public const string ALEX_STRING = "Alex";
	public const string DEREK_STRING = "Derek";
	public const string ZOE_STRING = "Zoe";
	public const string ALEX_WITH_MOVEMENT_STRING = "Alex With Movement";
	public const string DEREK_WITH_MOVEMENT_STRING = "Derek With Movement";
	public const string ZOE_WITH_MOVEMENT_STRING = "Zoe With Movement";
	public const string PLAYER_CENTRE_POINT = "\"Centre Point\"";
    public const string CAMERA_IGNORE_COLLISION_LAYER = "CameraCollisionIgnore";
	
	public const string LAUNCHER_STRING = "Launcher";
	public const string MOVING_PLATFORM_LAYER_STRING = "MovingPlatformLayer";
	public const string COLLIDE_WITH_MOVING_PLATFORM_LAYER_STRING = "CollideWithPlatform";
	public const string COLLIDE_WITH_MOVING_PLATFORM_STRING = "CollideWithMovingPlatforms";

	public const string PLAYER_PROJECTILE_STRING = "PlayerProjectile";
	public const string ENEMY_PROJECTILE_STRING = "EnemyProjectile";

	public const string CHECKPOINT_STRING = "Checkpoint";
	public const string CHECK_POINT_1_STRING = "CheckPoint_1";
	public const string CHECK_POINT_2_STRING = "CheckPoint_2";
	public const string CHECK_POINT_3_STRING = "CheckPoint_3";

	public const string LIGHT_PEG_TAG = "LightPeg";
	public const string MAIN_CAMERA_STRING = "MainCamera";
	public const string SOUND_MANAGER = "SoundManager";
    public const string COLLECTABLE_MANAGER = "CollectableManager";
    public const string HUD = "Hud";

    public const string WALL_TAG_STRING = "Wall";
    public const string MOVING_PLATFORM_TAG_STRING = "MovingPlatform";
	public const string MOVING_BLOCK_TAG_STRING = "MovingBlock";

	public const short NUMBER_OF_LEVELS = 1;

	public const string LOADING_SCREEN = "LoadingScreen";
	public const string LEVEL1_SECTION1 = "Level1_Section1";
	public const string LEVEL1_SECTION2 = "Level1_Section2";
	public const string LEVEL1_SECTION3 = "Level1_Section3";
	public const string LEVEL1_SECTIONBOSS = "Level1_SectionBoss";
	public const string GAME_OVER_SCREEN = "GameOverScreen";

	public const string NAV_AGENT = "NavMeshAgent";

	public const float LIGHT_PEG_FORCE = 60.0f;

	public const string RIGHT_ANGLE = "RightAngle";
	public const string LEFT_ANGLE = "LeftAngle";
	public const string FORWARD_ANGLE = "ForwardAngle";
	public const string BACK_ANGLE = "BackAngle";

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

	public struct HudImages
	{
//numbers
		public const string NUMBERS_PATH = "HUD textures/Numbers";

//standard hud images
		public const string LIGHT_PEG_HUD_IMAGE = "HUD_textures/Light_Bright";
		public const string LIFE_COUNT_IMAGE = "HUD_textures/heart";
		public const string CHECKPOINT_IMAGE = "HUD_textures/Checkpoint";
		public const string LEFT_HEALTH_BOARDER = "HUD_textures/HealthBoarders/PortraitBorderLeft";
		public const string RIGHT_HEALTH_BOARDER = "HUD_textures/HealthBoarders/PortraitBorderRight";

//Alex hud images
		public const string ALEX_FULL_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Alex/Alex";
		public const string ALEX_INJURED_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Alex/AlexInjured";
		public const string ALEX_CRITICAL_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Alex/AlexCritical";
		public const string ALEX_DEAD_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Alex/AlexDead";

//derek hud images
		public const string DEREK_FULL_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Derek/Derek";
		public const string DEREK_INJURED_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Derek/DerekInjured";
		public const string DEREK_CRITICAL_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Derek/DerekCritical";
		public const string DEREK_DEAD_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Derek/DerekDead";

//zoey hud images
		public const string ZOEY_FULL_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Zoe/Zoe";
		public const string ZOEY_INJURED_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Zoe/ZoeInjured";
		public const string ZOEY_CRITICAL_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Zoe/ZoeCritical";
		public const string ZOEY_DEAD_HEALTH_IMAGE = "HUD_textures/PlayerHealth/Zoe/ZoeDead";

//Puzzle pieces
		public const string PUZZLEPIECE_ZERO_IMAGE = "HUD_textures/Puzzle/Puzzle_00";
		public const string PUZZLEPIECE_ONE_IMAGE = "HUD_textures/Puzzle/Puzzle_01";
		public const string PUZZLEPIECE_TWO_IMAGE = "HUD_textures/Puzzle/Puzzle_02";
		public const string PUZZLEPIECE_THREE_IMAGE = "HUD_textures/Puzzle/Puzzle_03";
		public const string PUZZLEPIECE_FOUR_IMAGE = "HUD_textures/Puzzle/Puzzle_04";
		public const string PUZZLEPIECE_FIVE_IMAGE = "HUD_textures/Puzzle/Puzzle_05";
		public const string PUZZLEPIECE_SIX_IMAGE = "HUD_textures/Puzzle/Puzzle_06";
	}

	public struct Sounds
	{

		//COMMON
		public const string	JUMP = "Sounds/NewSounds/Alex/Alex_Double_Jump";
		public const string WALK = "Sounds/Common/footsteps_carpet_walk";
		public const string RUN_1 = "Sounds/NewSounds/Common/Footstep_1";
		public const string RUN_2 = "Sounds/NewSounds/Common/Footstep_2";
		public const string RUN_3 = "Sounds/NewSounds/Common/Footstep_3";
		public const string RUN_4 = "Sounds/NewSounds/Common/Footstep_4";
		public const string CHARACTER_DEATH = "Sounds/NewSounds/Common/Character_Death";
		public const string CHARACTER_RESPAWN = "Sounds/NewSounds/Common/Character_Respawn";

		public const string WEAPON_WOOSH = "Sounds/Common/Woosh";
		public const string COLLECTABLE_1 = "Sounds/NewSounds/Level/Pickup_LightPeg_1";
		public const string COLLECTABLE_2 = "Sounds/NewSounds/Level/Pickup_LightPeg_2";
		public const string COLLECTABLE_3 = "Sounds/NewSounds/Level/Pickup_LightPeg_3";
		public const string COLLECTABLE_4 = "Sounds/NewSounds/Level/Pickup_LightPeg_4";
		public const string COLLECTABLE_5 = "Sounds/NewSounds/Level/Pickup_LightPeg_5";
		public const string COLLECTABLE_50 = "Sounds/NewSounds/Level/Pickup_LightPeg_Fifty";
		public const string PUZZLE_PEICE = "Sounds/NewSounds/Level/Puzzle_Piece_Collected";
		public const string JUMPAD = "Sounds/NewSounds/Level/Trampoline_Bounce";
		public const string ZIPPER = "Sounds/Common/Zipper";
		public const string GATE_OPEN = "Sounds/Interactable/Gate_Down";
		public const string WEAPON_SWING_1 = "Sounds/NewSounds/Common/Attack_Swing_1";
		public const string WEAPON_SWING_2 = "Sounds/NewSounds/Common/Attack_Swing_2";
		public const string WEAPON_SWING_3 = "Sounds/NewSounds/Common/Attack_Swing_3";
		public const string AIR_ATTACK_SMASH = "Sounds/NewSounds/Common/Character_Smash_Attack";


		//LEVEL
		public const string CHECKPOINT_REACHED = "Sounds/NewSounds/Level/Checkpoint_Reached";
		public const string CONTACT_LEVER = "Sounds/NewSounds/Level/Contact_Lever";
		public const string LEVEL_COMPLETE = "Sounds/NewSounds/Level/Level_Complete";
		public const string TRAMPOLINE_BOUNCE = "Sounds/NewSounds/Level/Trampoline_Bounce";

		//Alex Sounds
		public const string ALEX_FIRST_WEAPON_HIT = "Sounds/Alex/First_Weapon_hit_Alex";
		public const string ALEX_SECOND_WEAPON_HIT = "Sounds/Alex/Second_Weapon_Hit_Alex";
		public const string ALEX_THIRD_WEAPON_HIT = "Sounds/Alex/Final_Weapon_hit_Alex";
		public const string ALEX_HURT = "Sounds/Alex/Alex_Painful_Grunt";
		public const string ALEX_JUMP = "Sounds/NewSounds/Alex/Alex_Double_Jump"; //TODO: replace with standard jump
		public const string ALEX_DOUBLE_JUMP = "Sounds/NewSounds/Alex/Alex_Double_Jump";
		public const string ALEX_DEATH = "Sounds/Alex/Alex_Death";
		public const string ALEX_SCRAPE_GROUND_ATTACK = "Sounds/NewSounds/Alex/Alex_Scrape_Ground_Attack";
		public const string ALEX_WHIRLWIND = "Sounds/NewSounds/Alex/Alex_Whirlwind";


		//Derek Sounds
		public const string DEREK_FIRST_WEAPON_HIT  =  "Sounds/Derek/Derek_First_Hit";
		public const string DEREK_SECOND_WEAPON_HIT =  "Sounds/Derek/Derek_Second_Hit";
		public const string DEREK_THIRD_WEAPON_HIT =  "Sounds/Derek/Derek_Third_Hit";
		public const string DEREK_THUNDER_CLAP = "Sounds/NewSounds/Derek/Derek_Attack_ThunderClap"; // 
		public const string DEREK_HURT =  "Sounds/Derek/Derek_Painful_Grunt";
		public const string DEREK_JUMP = "Sounds/NewSounds/Derek/Derek_Jump_Grunt"; 
		public const string DEREK_DEATH = "Sounds/Derek/Derek_Death";

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
		public const string MAGE_SHOOT =  "Sounds/NewSounds/Enemy/GnomeMage_Attack";
		public const string MAGE_MOVE =  "Sounds/NewSounds/Enemy/GnomeMage_Movement"; 
		public const string MAGE_SHIELD_BREAK =  "Sounds/NewSounds/Enemy/GnomeMage_Shield_Break"; 
		public const string MAGE_HIT =  "Sounds/NewSounds/Enemy/GnomeMage_TakesDamage";

		public const string SPINTOP_HIT =  "Sounds/NewSounds/Enemy/SpinTop_Gets_Hit";
		public const string SPINTOP_CHARGE =  "Sounds/NewSounds/Enemy/SpinTop_Charge";

		public const string FURBULL_HOP =  "Sounds/NewSounds/Enemy/Furbull_Hop";
		public const string FURBULL_ATTACK = "Sounds/NewSounds/Enemy/Furbull_Attack_Swing";

		public const string ENEMY_DEATH =  "Sounds/NewSounds/Enemy/Enemy_Death";

		public const string SONG_SECTION_1 = "Sounds/NewSounds/Music/Section1_Song";
		public const string SONG_SECTION_2 = "Sounds/NewSounds/Music/Section2_Song";
		public const string SONG_SECTION_3 = "Sounds/NewSounds/Music/Section3_Song";
		public const string SONG_MAIN_MENU = "Sounds/NewSounds/Music/MainMenuSong";
	}

	// Enemy Constants
	public const string ENEMY_STRING = "Enemy";
	public const float MAGE_ATTACK_RANGE = 10.0f;
    public const float FURBULL_ATTACK_RANGE = 1.5f;
    public const float SPIN_TOP_ATTACK_RANGE = 15.0f;
    public const float KNOCKBACK_MULTIPLIER = 20.0f;


	public const string MOVEMENT_STRING = "Movement";
	public const string CLONING_MOVEMENT_STRING = "Cloning Movement";
	public const string CLONED_MOVEMENT_STRING = "Cloned Movement";
	public const string CHARGE_MOVEMENT_STRING = "Charge Movement";
	public const string BUILDING_CHARGE_STRING = "Building Charge Movement";
	public const string KNOCKED_BACK_MOVEMENT_STRING = "Knocked Back Movement";
	public const string HIT_BY_PLAYER_MOVEMENT_STRING = "Hit By Player Movement";
	public const string TARGETING_STRING = "Targeting";
	public const string ENTER_COMBAT_STRING = "Enter Combat";
	public const string LEAVE_COMBAT_STRING = "Leave Combat";
	public const string COMBAT_STRING = "Combat";
	public const string CLONED_COMBAT_STRING = "Cloned Combat";
	public const string DEATH_STRING = "Death";

	public const string COMPONENTS_STRING = "Components";

	public const string BASE_BEHAVIOUR_STRING = "Base Behaviour";
	public const string COMBAT_BEHAVIOUR_STRING = "Combat Behaviour";
	public const string IDLE_BEHAVIOUR_STRING = "Idle Behaviour";
	public const string CHASE_BEHAVIOUR_STRING = "Chase Behaviour";
	public const string DEAD_BEHAVIOUR_STRING = "Dead Behaviour";

	public static string[] MOVEMENT_COMPONENTS_ARRAY = {"ArcWhileMovingBackwards",
									 "BuildUpChargeMovement",
									 "ChargeMovement",
									 "ChaseTargetMovement",
									 "JumpBackMovement",
									 "KnockedBackMovement",
									 "MovementAroundNodes",
									 "NoMovement"};

	public static string[] COMBAT_COMPONENTS_ARRAY = {"BasicProjectileCombat",
								   "CollisionCombat"};

	public static string[] DEATH_COMPONENTS_ARRAY = {"None"};

	public static string[] ENTER_COMBAT_COMPONENTS_ARRAY = {"ProximityAggro"};

	public static string[] LEAVE_COMBAT_COMPONENTS_ARRAY = {"ProximityAggroLeave"};

	public static string[] TARGETING_COMPONENTS_ARRAY = {"BasicTargeting"};
}
