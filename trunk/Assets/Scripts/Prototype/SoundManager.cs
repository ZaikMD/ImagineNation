using UnityEngine;
using System.Collections;



//The Enum for the sounds. They will have to be IN THE SAME ORDER AS YOU LOAD THEM
public enum Sounds
{
	ArmyMenTalk = 0,
	BoxingGloveHit= 1,
	BoxingGloveImpact= 2,
	BreakingObject= 3,
	CharacterSwitch = 4,
	NerfGunBullet = 5,
	NerfGunPlatform = 6,
	RcCar = 7,
	SeesawJump= 8,
	StickyHandShot = 9,
	Trampoline = 10,

	Count
};


public enum Songs
{
	BackgroundSong = 0,
	Music2 = 1,
	Count
};

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {


	public static SoundManager Instance{ get; private set; }
	//Use for initialization

	private  AudioClip[] m_Sounds;
	private AudioClip[] m_Songs;

	public float m_Volume;

	private int m_CurrentSong;
	private int m_SongPosition;
	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			//If there's another instance, destroy it... there can only be one prince of darkness
			Destroy(gameObject);
		}

		Instance = this;

		DontDestroyOnLoad (gameObject);

	}




	// Use this for initialization
	void Start ()
	{
		//Create the sound array
		m_Sounds = new AudioClip[(int)Sounds.Count];

		//Create the Songs array
		m_Songs = new AudioClip[(int)Songs.Count];

		// Load ALL sounds here. MAKE SURE THEY ARE IN THE SAME ORDER AS THE ENUM
		m_Sounds[0] = Resources.Load<AudioClip>("Sound Effects/ArmyMenTalk");
		m_Sounds[1] = Resources.Load<AudioClip>("Sound Effects/BoxingGloveHit");
		m_Sounds[2] = Resources.Load<AudioClip>("Sound Effects/BoxingGloveImpact");
		m_Sounds[3] = Resources.Load<AudioClip>("Sound Effects/BreakingObject");
		m_Sounds[4] = Resources.Load<AudioClip>("Sound Effects/CharacterSwitch");
		m_Sounds[5] = Resources.Load<AudioClip>("Sound Effects/NerfGunBullet");
		m_Sounds[6] = Resources.Load<AudioClip>("Sound Effects/NerfGunPlatform");
		m_Sounds[7] = Resources.Load<AudioClip>("Sound Effects/RcCar");
		m_Sounds[8] = Resources.Load<AudioClip>("Sound Effects/SeesawJump");
		m_Sounds[9] = Resources.Load<AudioClip>("Sound Effects/StickyHandShot");
		m_Sounds[10] = Resources.Load<AudioClip>("Sound Effects/Trampoline");

		//Load Music here. Have same order as enum!
		m_Songs [0] = Resources.Load<AudioClip> ("Sound Effects/BackgroundSong"); 
		m_Songs [1] = Resources.Load<AudioClip> ("Sound Effects/Music2");

		audio.clip = m_Songs [m_CurrentSong];
		audio.Play ();

		m_SongPosition = m_CurrentSong;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//makes sure there is background music player
		if(audio.isPlaying == false)
		{
			m_CurrentSong++;
			audio.clip = m_Songs[m_CurrentSong];
			audio.Play();

		}

		if(m_CurrentSong >= 1)
		{
			m_CurrentSong = -1;
		}
	}
	 //Call This from any fucntion with the appropriate sound name
	public void playSound(Sounds sound, Vector3 position)
	{
		AudioSource.PlayClipAtPoint (m_Sounds [(int)sound], position, m_Volume);
	}

	public void playSong(Songs song, float volume)
	{
		m_CurrentSong = (int)song;
		m_SongPosition = m_CurrentSong;
	}




}
