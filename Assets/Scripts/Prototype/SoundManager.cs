using UnityEngine;
using System.Collections;



//The Enum for the sounds. They will have to be IN THE SAME ORDER AS YOU LOAD THEM
public enum Sounds
{
	testSound = 0,
	Count
};


public enum Songs
{
	testSong = 0,
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

		//TODO: Load ALL sounds here. MAKE SURE THEY ARE IN THE SAME ORDER AS THE ENUM
		m_Sounds[0] = Resources.Load<AudioClip>("Blip_Select");

		m_Songs [0] = Resources.Load<AudioClip> ("Music");

		audio.clip = m_Songs [m_CurrentSong];
		audio.Play ();

		m_SongPosition = m_CurrentSong;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(audio.isPlaying == false)
		{
			audio.clip = m_Songs[m_CurrentSong];
			audio.Play();
		}

		if(m_CurrentSong > m_SongPosition)
		{
			m_CurrentSong = m_SongPosition;
		}
	}
	 //Call This from any fucntion with the appropriate sound name
	public void playSound(Sounds sound, Vector3 position, float volume)
	{
		AudioSource.PlayClipAtPoint (m_Sounds [(int)sound], position, volume);
	}

	public void playSong(Songs song, float volume)
	{
		m_CurrentSong = (int)song;
		m_SongPosition = m_CurrentSong;
	}




}
