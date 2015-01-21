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
	WeaponWoosh,
	JumpPad,
	Zipper,
	GateOpen,

	//Alex
	AlexHitOne,
	AlexHitTwo,
	AlexHitThree,
	AlexHurt,
	AlexDeath,
	AlexJump,

	//Derek
	DerekHitOne,
	DerekHitTwo,
	DerekHitThree,
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

	//Enemies
	MageAttack,
	MageHit
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


    Dictionary<int, AudioClip> m_SoundDictionary = new Dictionary<int, AudioClip>();
	//Variables for class
    List<SoundSourceMover> m_NonAutoDestroySources = new List<SoundSourceMover>();

//TODO: load which players are one and two
	public Transform m_PlayerOne;
	public Transform m_PlayerTwo;

	/// <summary>
	/// Raises the level load event.
	/// Loads in all the sounds from resources folder
	/// </summary>
    void OnLevelLoad()
    { 
        //Load all sounds
        loadMenuSounds();//menu sounds load instantly since they need to be used immediately
        loadOtherSounds();//other sounds load asynronously to help reduce lag on scene initilization
    }

    /// <summary>
    /// Start this instance.
	/// using Start to load for testing, OnLevelLoad in future
    /// </summary>
	void Start ()
    {
		//TODO: load which players are player one and two
		m_PlayerOne = getPlayerTransform (GameData.Instance.PlayerOneCharacter);
		m_PlayerTwo = getPlayerTransform (GameData.Instance.PlayerTwoCharacter);

#if DEBUG || UNITY_EDITOR
		//TODO: Delete for finale product, Onload will handle. OnLoad does not run when playing scene in editor
		//Load all sounds
        loadMenuSounds();
        loadOtherSounds();
#endif
	}

	/// <summary>
	/// Gets the player transform by c
	/// </summary>
	/// <returns>The player transform.</returns>
	/// <param name="charater">Charater.</param>
	Transform getPlayerTransform(Characters charater)
	{
		switch(charater)
		{
			case Characters.Alex:
			return GameObject.Find(Constants.ALEX_WITH_MOVEMENT_STRING).transform;

			case Characters.Derek:
			return GameObject.Find(Constants.DEREK_WITH_MOVEMENT_STRING).transform;

			case Characters.Zoe:
			return GameObject.Find(Constants.ZOE_WITH_MOVEMENT_STRING).transform;

			default:
#if DEBUG || UNITY_EDITOR
			Debug.LogError("Enum is out of range");
#endif
			return null;
		}
	}

	/// <summary>
	/// This function plays a sound from the location specified
	/// </summary>
	/// <param name="Location">Pass in a vector3 for the location of the sound to play from don't use if it needs to move with them.</param>
	/// <param name="The sound that is played, it is an enum, to select a sound, Type Sounds.insertSoundNameHere, Sound names are all one word">.</param>
	public void playSound(Transform location, Sounds sound)
	{
        //check if the sound is done loading
        if (!m_SoundDictionary.ContainsKey((int)sound))
            return;
        //create the actual source
        GameObject soundObject = new GameObject();
        SoundSourceMover soundSourceMover = soundObject.AddComponent<SoundSourceMover>();

		//this class takes the enum passed in and passes it to anouther function to get all data it needs.
		AudioInfo tempSoundInfo = getClipFromList(sound);

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
        if (!m_SoundDictionary.ContainsKey((int)sound))
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

	/// <summary>
	/// Checks sent in enum, returns apropriote data 
	/// such as if it is a one shot and the AudioClip
	/// </summary>
	/// <returns>The clip from list.</returns>
	/// <param name="sound"> An enum of the Sound. Put Sounds.NameOfSound</param>
    AudioInfo getClipFromList(Sounds sound)
    {
        AudioInfo tempAudioInfo = new AudioInfo();
        tempAudioInfo.m_AudioClip = m_SoundDictionary[(int)sound];
        
        switch(sound)
        {
	//Common Sounds
        case Sounds.Jump:
            tempAudioInfo.OneShot = false;
            break;

        case Sounds.Walk:
            tempAudioInfo.OneShot = false;
            break;

        case Sounds.Run:
            tempAudioInfo.OneShot = false;
            break;

		case Sounds.Collectable:
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

		case Sounds.AlexHurt:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexDeath:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.AlexJump:
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

		case Sounds.MageAttack:
			tempAudioInfo.OneShot = true;
			break;

		case Sounds.MageHit:
			tempAudioInfo.OneShot = true;
			break;

		default:
            tempAudioInfo.OneShot = false;
#if DEBUG || UNITY_EDITOR
			Debug.LogError("No regonized sound passed in");
#endif            
            break;
        }
		return tempAudioInfo;
    }

    void loadMenuSounds()
    {
        
    }

    void loadOtherSounds()
    {
        //Common Sounds
        loadSound((int)Sounds.Jump, Constants.Sounds.ALEX_JUMP);
        loadSound((int)Sounds.Walk, Constants.Sounds.WALK);
        loadSound((int)Sounds.Run, Constants.Sounds.RUN);
        loadSound((int)Sounds.WeaponWoosh, Constants.Sounds.WEAPON_WOOSH);
        loadSound((int)Sounds.Collectable, Constants.Sounds.COLLECTABLE);
        loadSound((int)Sounds.JumpPad, Constants.Sounds.JUMPAD);
        loadSound((int)Sounds.GateOpen, Constants.Sounds.GATE_OPEN);

        //Alex Sounds
        loadSound((int)Sounds.AlexHitOne, Constants.Sounds.ALEX_FIRST_WEAPON_HIT);
        loadSound((int)Sounds.AlexHitTwo, Constants.Sounds.ALEX_SECOND_WEAPON_HIT);
        loadSound((int)Sounds.AlexHitThree, Constants.Sounds.ALEX_THIRD_WEAPON_HIT);
        loadSound((int)Sounds.AlexHurt, Constants.Sounds.ALEX_HURT);
        loadSound((int)Sounds.AlexDeath, Constants.Sounds.ALEX_DEATH);
        loadSound((int)Sounds.AlexJump, Constants.Sounds.ALEX_JUMP);

        //Derek Sounds
        loadSound((int)Sounds.DerekHitOne, Constants.Sounds.DEREK_FIRST_WEAPON_HIT);
        loadSound((int)Sounds.DerekHitTwo, Constants.Sounds.DEREK_SECOND_WEAPON_HIT);
        loadSound((int)Sounds.DerekHitThree, Constants.Sounds.DEREK_THIRD_WEAPON_HIT);
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

        //Enimies sounds
		loadSound((int)Sounds.MageAttack, Constants.Sounds.MAGE_SHOOT);
		loadSound((int)Sounds.MageHit, Constants.Sounds.MAGE_HIT);
    }

    void loadSound(int key, string filePath)
    {
        StartCoroutine(loadSoundAsync(key, filePath));
    }

    IEnumerator loadSoundAsync(int key, string filePath)
    {
        if (!m_SoundDictionary.ContainsKey(key))
        {
            //Debug.Log(++totalCoroutines);
            ResourceRequest resource = Resources.LoadAsync(filePath);

            while (resource.isDone == false)
            {
                yield return null;
            }

            //Debug.Log(--totalCoroutines);
            m_SoundDictionary.Add(key, (AudioClip)resource.asset);
        }
#if UNITY_EDITOR || DEBUG
        else
        {
            Debug.Log("the key: " + key + "   for sound: " + filePath + "   already exists");
        }
#endif
        yield return null;        
    }

    void loadSoundNow(int key, string filePath)
    {
        if (!m_SoundDictionary.ContainsKey(key))
        {
            m_SoundDictionary.Add(key, (AudioClip)Resources.Load(filePath));
        }
#if UNITY_EDITOR || DEBUG
        else
        {
            Debug.Log("the key: " + key + "   for sound: " + filePath + "   already exists");
        }
#endif
    }
}
