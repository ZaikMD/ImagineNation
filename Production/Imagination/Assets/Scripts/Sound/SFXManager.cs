using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Created By: Kole
 *  
 * This Script loads all sound effects and tells the audio sources to play
 * a passed in sound.
 * 
 * To Play a sound effect, First get a reference to this script (there should be a
 * empty game Object in your scene with the SFXManager script and the Sound Manager tag).
 * call the Play Sound function and pass in ethier the game object that will be the origin,
 * or a vector3 repersenting the posityion of the orgin for the first argument.
 * for the second argument, pass in a sound as an enum of Sound( Sounds.nameOfSound )
 * 
 * 
 * Oct, 1, 2014, kole
 * Updated the class to work with two cameras. to play a sound will be done the same way
 * this now gets the player who is closest and gets a value based of off the distance to the player
 * 
 * 
 * Oct, 8, 2014, kole
 * Updated the class to use constants instead of strings
 * 
 * jan 16 2015 kris
 * made the class use a dictionary for all of the variables
 * and added loading with coroutines
 * 
 * jan 20 2015 kris
 * i think i fixed loading with coroutines
 * made script a unity styled singleton
 */


//The name of all the sounds are below
public enum Sounds
{
	//Common
    Jump,
    Walk,
    Run, 
	Collectable, 
	PuzzlePeice,
	WeaponWoosh,
	JumpPad, 
	Zipper,
	GateOpen,
	CharacterDeath, 
	CharacterRespawn, 
	LiveIncrement,
	AirSmashAttack,

	//Level
	Checkpoint, 
	LeverHit, 
	LevelComplete, 

	//Alex
	AlexHitOne,
	AlexHitTwo,
	AlexHitThree,
	AlexScrapeAttack,  
	AlexWhirlWind, 
	AlexHurt,
	AlexDeath,
	AlexJump, 
	AlexDoubleJump, 

	//Derek7
	DerekHitOne,
	DerekHitTwo,
	DerekHitThree,
	DerekThunderClap,
	DerekHurt,
	DerekDeath,
	DerekJump,

	//Zoey
	ZoeyHitOne,
	ZoeyHitTwo,
	ZoeyHitThree,
	ZoeyHurt,
	ZoeyDeath,
	ZoeyJump,
	ZoeyOpenWings,
	ZoeyCloseWings,
	ZoeyDeployedWings,
	ZoeyAOE,

	//Enemies
	MageAttack, 
	MageMove,  
	MageShieldBreak, 
	MageHit, 

	SpinTopHit, 
	SpinTopCharge, 

	FurbullHop, 
	FurbullAttack,

	EnemyDeath,

	//music
	SongSection1,
	SongSection2,
	SongSection3,
	SongMainMenu
}

//this struct holds all the info needed to determine how to play are sounds
public struct AudioInfo
{
    public AudioClip m_AudioClip;
    public bool OneShot;    
}

/// <summary>
/// the SFX manager class plays all SFX for our game,
/// other classes that will use this class will have a
/// reference to it and call the PlaySound() Function.
/// </summary>
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    //ID is used to ensure that the older GamaData is kept in the case of duplicates
    static int m_InstanceCounter = 0;
    public int m_ID = int.MaxValue;
    public int ID
    {
        get { return m_ID; }
        private set { m_ID = value; }
    }

    void Awake()
    {
        ID = m_InstanceCounter++;

        //if theres another instance (there shouldnt be) destroy this
        if (Instance != null && Instance != this)
        {
            if (ID > Instance.ID)
            {
                //destroy all other instances
                Destroy(gameObject);
                return;
            }
            else
            {
                Destroy(Instance.gameObject);
            }
        }

        //set the instance
        Instance = this;

        //prevents this object being destroyed between scene loads
        DontDestroyOnLoad(this.gameObject);
    }
    //================================================================================ 


    Dictionary<int, List<AudioClip>> m_SoundDictionary = new Dictionary<int, List<AudioClip>>();
    List<string> m_LoadedSounds = new List<string>();
	//Variables for class
    List<SoundSourceMover> m_NonAutoDestroySources = new List<SoundSourceMover>();

	/// <summary>
	/// Raises the level load event.
	/// Loads in all the sounds from resources folder
	/// </summary>
 
	void OnEnable()
    { 
        //Load all sounds
        loadMenuSounds();//menu sounds load instantly since they need to be used immediately
		loadMusicSounds();
        loadOtherSounds();//other sounds load asynronously to help reduce lag on scene initilization
	//	PlaySong();
    }


    bool soundExists(Sounds key)
    {
        return soundExists((int)key);
    }

    bool soundExists(int key)
    {
        return m_SoundDictionary.ContainsKey(key) && m_SoundDictionary[key].Count > 0;
    }

	/// <summary>
	/// This function plays a sound from the location specified
	/// </summary>
	/// <param name="Location">Pass in a vector3 for the location of the sound to play from don't use if it needs to move with them.</param>
	/// <param name="The sound that is played, it is an enum, to select a sound, Type Sounds.insertSoundNameHere, Sound names are all one word">.</param>
	public void playSound(Transform location, Sounds sound)
	{
        //check if the sound is done loading
         if (!soundExists(sound))
		{
			Debug.Log("no sound exsist for " + sound);
            return;
		}
        //create the actual source
        GameObject soundObject = new GameObject();
        SoundSourceMover soundSourceMover = soundObject.AddComponent<SoundSourceMover>();

		//this class takes the enum passed in and passes it to anouther function to get all data it needs.
		AudioInfo tempSoundInfo = getClipFromList(sound);

		soundObject.name = tempSoundInfo.m_AudioClip.name;

        //will the sound remove itself once done?
		if (tempSoundInfo.OneShot)
        {
            soundSourceMover.initialize(location, transform, tempSoundInfo.m_AudioClip, tempSoundInfo.OneShot);
        }
        else
        {//check to see if it already exsists
            for(int i = 0; i < m_NonAutoDestroySources.Count; i++)
            {
                if (m_NonAutoDestroySources[i].AudioSource.clip == tempSoundInfo.m_AudioClip && m_NonAutoDestroySources[i].SourceObject == location)
                {
                    if (!m_NonAutoDestroySources[i].AudioSource.isPlaying)
                    {
                        m_NonAutoDestroySources[i].AudioSource.Play();
                    }
                    Destroy(soundObject);
                    return;
                }
            }
            //need to add it
            soundSourceMover.initialize(location, transform, tempSoundInfo.m_AudioClip, tempSoundInfo.OneShot);
            m_NonAutoDestroySources.Add(soundSourceMover);
        }
	}	

	/// <summary>
	/// Stops the sound.
	/// </summary>
	/// <param name="objectPlayingTheSound">Object playing the sound.</param>
    public void stopSound(Transform location, Sounds sound, bool destroy = false)
	{
        if (!soundExists(sound))
            return;
        AudioInfo tempSoundInfo = getClipFromList(sound);
        for (int i = 0; i < m_NonAutoDestroySources.Count; i++)
        {
            if (m_NonAutoDestroySources[i].AudioSource.clip == tempSoundInfo.m_AudioClip && m_NonAutoDestroySources[i].SourceObject == location)
            {
                if (!destroy)
                {
                    m_NonAutoDestroySources[i].AudioSource.Stop();
                    return;
                }
                GameObject.Destroy(m_NonAutoDestroySources[i].gameObject);
                m_NonAutoDestroySources.RemoveAt(i);
                return;
            }
        } 
#if DEBUG || UNITY_EDITOR
        Debug.Log("sound not found");
#endif
    }

    AudioClip getSound(Sounds key)
    {
        return getSound((int)key);
    }

    AudioClip getSound(int key)
    {
        return m_SoundDictionary[key][Random.Range(0, m_SoundDictionary[key].Count)];
    }

	/// <summary>
	/// Checks sent in enum, returns apropriote data 
	/// such as if it is a one shot and the AudioClip
	/// </summary>
	/// <returns>The clip from list.</returns>
	/// <param name="sound"> An enum of the Sound. Put Sounds.NameOfSound</param>
    AudioInfo getClipFromList(Sounds sound)
    {
        AudioInfo tempAudioInfo = new AudioInfo();
        tempAudioInfo.m_AudioClip = getSound(sound);
        
        switch(sound)
        {
	//Common Sounds
        case Sounds.Jump:
            tempAudioInfo.OneShot = true;
            break;

        case Sounds.Walk:
            tempAudioInfo.OneShot = false;
            break;

        case Sounds.Run:
            tempAudioInfo.OneShot = true;
            break;

		case Sounds.Collectable:
            tempAudioInfo.OneShot = true;
            break;

		case Sounds.PuzzlePeice:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.WeaponWoosh:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.JumpPad:
			tempAudioInfo.OneShot = true;
			break;
	
		case Sounds.GateOpen:
			tempAudioInfo.OneShot = false;
			break;

		case Sounds.LeverHit:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.LevelComplete:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.LiveIncrement:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.CharacterDeath:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.CharacterRespawn:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AirSmashAttack:
			tempAudioInfo.OneShot = true;
			break;

			//Alex Sounds
		case Sounds.AlexHitOne:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexHitTwo:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexHitThree:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexScrapeAttack:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexWhirlWind:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexHurt:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexDeath:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexJump:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexDoubleJump:
			tempAudioInfo.OneShot = true;
			break;

	//Derek Sounds
		case Sounds.DerekHitOne:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.DerekHitTwo:
			tempAudioInfo.OneShot = true;
			break;
		
		case Sounds.DerekHitThree:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.DerekThunderClap:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.DerekHurt:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.DerekDeath:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.DerekJump:
			tempAudioInfo.OneShot = true;;
			break;


	//Zoey Sounds
		case Sounds.ZoeyHitOne:
			tempAudioInfo.OneShot = true;
			break;
			
		case Sounds.ZoeyHitTwo:
			tempAudioInfo.OneShot = true;
			break;
			
		case Sounds.ZoeyHitThree:
			tempAudioInfo.OneShot = true;
			break;
			
		case Sounds.ZoeyHurt:
			tempAudioInfo.OneShot = true;
			break;


		case Sounds.ZoeyDeath:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.ZoeyJump:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.ZoeyOpenWings:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.ZoeyCloseWings:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.ZoeyDeployedWings:
			tempAudioInfo.OneShot = false;
			break;

		case Sounds.ZoeyAOE:
			tempAudioInfo.OneShot = true;
			break;


	//Enemies
		case Sounds.MageAttack:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.MageShieldBreak:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.MageHit:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.MageMove:
			tempAudioInfo.OneShot = true;
			break;



		case Sounds.SpinTopCharge:
			tempAudioInfo.OneShot = false;
			break;

		case Sounds.SpinTopHit:
			tempAudioInfo.OneShot = true;
			break;

		
		case Sounds.FurbullHop:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.FurbullAttack:
			tempAudioInfo.OneShot = true;
			break;


		case Sounds.EnemyDeath:
			tempAudioInfo.OneShot = true;
			break;


		case Sounds.SongSection1:
			tempAudioInfo.OneShot = false;
			break;

		case Sounds.SongSection2:
			tempAudioInfo.OneShot = false;
			break; 

		case Sounds.SongSection3:
			tempAudioInfo.OneShot = false;
			break; 

		case Sounds.SongMainMenu:
			tempAudioInfo.OneShot = false;
			break;


		default:
            tempAudioInfo.OneShot = false;
#if DEBUG || UNITY_EDITOR
			Debug.LogError("No regonized sound passed in");
			Debug.Log(sound);
#endif            
            break;
        }
		return tempAudioInfo;
    }

    void loadMenuSounds()
    {
        
    }

	void loadMusicSounds()
	{
		switch (GameData.Instance.CurrentSection)
		{
			case Sections.None:
			loadSoundNow((int)Sounds.SongMainMenu, Constants.Sounds.SONG_MAIN_MENU);
			break;

			case Sections.Sections_1:
			loadSoundNow((int)Sounds.SongSection1, Constants.Sounds.SONG_SECTION_1);
			break;

			case Sections.Sections_2:
			loadSoundNow((int)Sounds.SongSection2, Constants.Sounds.SONG_SECTION_2);
			break;

			case Sections.Sections_3:
			loadSoundNow((int)Sounds.SongSection3, Constants.Sounds.SONG_SECTION_3);
			break;

			default:
			loadSoundNow((int)Sounds.SongMainMenu, Constants.Sounds.SONG_MAIN_MENU);
			break;
		}
	}

	public void PlaySong()
	{
		switch(GameData.Instance.CurrentSection)
		{
		case Sections.None:
			playSound(this.transform, Sounds.SongMainMenu);
			break;
			
		case Sections.Sections_1:
			playSound(this.transform, Sounds.SongSection1);
			break;
			
		case Sections.Sections_2:
			playSound(this.transform, Sounds.SongSection2);
			break;
			
		case Sections.Sections_3:
			playSound(this.transform, Sounds.SongSection3);
			break;

		default:
			playSound(this.transform, Sounds.SongMainMenu);
			break;
		}	
	}

    void loadOtherSounds()
    {
        //Common Sounds
//        loadSound((int)Sounds.Jump, Constants.Sounds.ALEX_JUMP);
        loadSound((int)Sounds.Walk, Constants.Sounds.WALK);
        loadSound((int)Sounds.Run, Constants.Sounds.RUN_1);
		loadSound((int)Sounds.Run, Constants.Sounds.RUN_2);
		loadSound((int)Sounds.Run, Constants.Sounds.RUN_3);
		loadSound((int)Sounds.Run, Constants.Sounds.RUN_4);
        loadSound((int)Sounds.WeaponWoosh, Constants.Sounds.WEAPON_SWING_1);
		loadSound((int)Sounds.WeaponWoosh, Constants.Sounds.WEAPON_SWING_2);
		loadSound((int)Sounds.WeaponWoosh, Constants.Sounds.WEAPON_SWING_3);
        loadSound((int)Sounds.Collectable, Constants.Sounds.COLLECTABLE_1);
		loadSound((int)Sounds.Collectable, Constants.Sounds.COLLECTABLE_2);
		loadSound((int)Sounds.Collectable, Constants.Sounds.COLLECTABLE_3);
		loadSound((int)Sounds.Collectable, Constants.Sounds.COLLECTABLE_4);
		loadSound((int)Sounds.Collectable, Constants.Sounds.COLLECTABLE_5);
		loadSound ((int)Sounds.LiveIncrement, Constants.Sounds.COLLECTABLE_50);
		loadSound((int)Sounds.PuzzlePeice, Constants.Sounds.PUZZLE_PEICE);
        loadSound((int)Sounds.JumpPad, Constants.Sounds.JUMPAD);
        loadSound((int)Sounds.GateOpen, Constants.Sounds.GATE_OPEN);
		loadSound((int)Sounds.CharacterRespawn, Constants.Sounds.CHARACTER_RESPAWN);
		loadSound((int)Sounds.CharacterDeath, Constants.Sounds.CHARACTER_DEATH);
		loadSound ((int)Sounds.AirSmashAttack, Constants.Sounds.AIR_ATTACK_SMASH);

		//Level
		loadSound((int)Sounds.LevelComplete, Constants.Sounds.LEVEL_COMPLETE);
		loadSound((int)Sounds.LeverHit, Constants.Sounds.CONTACT_LEVER);
		loadSound((int)Sounds.Checkpoint, Constants.Sounds.CHECKPOINT_REACHED);

        //Alex Sounds
        loadSound((int)Sounds.AlexHitOne, Constants.Sounds.ALEX_FIRST_WEAPON_HIT);
        loadSound((int)Sounds.AlexHitTwo, Constants.Sounds.ALEX_SECOND_WEAPON_HIT);
        loadSound((int)Sounds.AlexHitThree, Constants.Sounds.ALEX_THIRD_WEAPON_HIT);
		loadSound((int)Sounds.AlexScrapeAttack, Constants.Sounds.ALEX_SCRAPE_GROUND_ATTACK);
		loadSound((int)Sounds.AlexWhirlWind, Constants.Sounds.ALEX_WHIRLWIND);
        loadSound((int)Sounds.AlexHurt, Constants.Sounds.ALEX_HURT);
        loadSound((int)Sounds.AlexDeath, Constants.Sounds.ALEX_DEATH);
        loadSound((int)Sounds.AlexJump, Constants.Sounds.ALEX_JUMP);
		loadSound((int)Sounds.AlexDoubleJump, Constants.Sounds.ALEX_DOUBLE_JUMP);

        //Derek Sounds
        loadSound((int)Sounds.DerekHitOne, Constants.Sounds.DEREK_FIRST_WEAPON_HIT);
        loadSound((int)Sounds.DerekHitTwo, Constants.Sounds.DEREK_SECOND_WEAPON_HIT);
        loadSound((int)Sounds.DerekHitThree, Constants.Sounds.DEREK_THIRD_WEAPON_HIT);
		loadSound ((int)Sounds.DerekThunderClap, Constants.Sounds.DEREK_THUNDER_CLAP);
        loadSound((int)Sounds.DerekHurt, Constants.Sounds.DEREK_HURT);
        loadSound((int)Sounds.DerekDeath, Constants.Sounds.DEREK_DEATH);
        loadSound((int)Sounds.DerekJump, Constants.Sounds.DEREK_JUMP);

        //Zoey Sounds
        loadSound((int)Sounds.ZoeyHitOne, Constants.Sounds.ZOEY_FIRST_WEAPON_HIT);
        loadSound((int)Sounds.ZoeyHitTwo, Constants.Sounds.ZOEY_SECOND_WEAPON_HIT);
        loadSound((int)Sounds.ZoeyHitThree, Constants.Sounds.ZOEY_THIRD_WEAPON_HIT);
        loadSound((int)Sounds.ZoeyHurt, Constants.Sounds.ZOEY_HURT);
        loadSound((int)Sounds.ZoeyDeath, Constants.Sounds.ZOEY_DEATH);
        loadSound((int)Sounds.ZoeyJump, Constants.Sounds.ZOEY_JUMP);
        loadSound((int)Sounds.ZoeyOpenWings, Constants.Sounds.ZOEY_WINGS_OPEN);
        loadSound((int)Sounds.ZoeyCloseWings, Constants.Sounds.ZOEY_WINGS_CLOSE);
		loadSound((int)Sounds.ZoeyDeployedWings, Constants.Sounds.ZOEY_WINGS_DEPLOY);
		loadSound ((int)Sounds.ZoeyAOE, Constants.Sounds.ZOEY_ATTACK_AOE);


        //Enimies sounds
		loadSound((int)Sounds.MageAttack, Constants.Sounds.MAGE_SHOOT);
		loadSound((int)Sounds.MageHit, Constants.Sounds.MAGE_HIT);
		loadSound((int)Sounds.MageMove, Constants.Sounds.MAGE_MOVE);
		loadSound((int)Sounds.MageShieldBreak, Constants.Sounds.MAGE_SHIELD_BREAK);

		loadSound((int)Sounds.SpinTopCharge, Constants.Sounds.SPINTOP_CHARGE);
		loadSound((int)Sounds.SpinTopHit, Constants.Sounds.SPINTOP_HIT);

		loadSound((int)Sounds.FurbullHop, Constants.Sounds.FURBULL_HOP);
		loadSound((int)Sounds.FurbullAttack, Constants.Sounds.FURBULL_ATTACK);

		loadSound((int)Sounds.EnemyDeath, Constants.Sounds.ENEMY_DEATH);
    }

    void loadSound(int key, string filePath)
    {
        StartCoroutine(loadSoundAsync(key, filePath));
    }

    IEnumerator loadSoundAsync(int key, string filePath)
    {
        if (!m_SoundDictionary.ContainsKey(key))
        {
            m_SoundDictionary.Add(key, new List<AudioClip>());
        }

        if (!filePathUsed(filePath))
        {
            m_LoadedSounds.Add(filePath);

            ResourceRequest resource = Resources.LoadAsync(filePath);

            while (resource.isDone == false)
            {
                yield return null;
            }
            m_SoundDictionary[key].Add((AudioClip)resource.asset);
        }
#if UNITY_EDITOR || DEBUG
        else
        {
            Debug.Log("the Path: " + filePath + "  has already been used");
        }
#endif
        yield return null;        
    }

    void loadSoundNow(int key, string filePath)
    {
        if (!m_SoundDictionary.ContainsKey(key))
        {
            m_SoundDictionary.Add(key, new List<AudioClip>());
        }

        if (!filePathUsed(filePath))
        {
            m_LoadedSounds.Add(filePath);

            m_SoundDictionary[key].Add((AudioClip)Resources.Load(filePath));
        }
#if UNITY_EDITOR || DEBUG
        else
        {
            Debug.Log("the Path: " + filePath + "  has already been used");
        }
#endif
    }

    bool filePathUsed(string filePath)
    {
        for(int i=0; i < m_LoadedSounds.Count; i++)
        {
            if(m_LoadedSounds[i].CompareTo(filePath) == 0)
            {
                return true;
            }
        }
        return false;
    }
}
