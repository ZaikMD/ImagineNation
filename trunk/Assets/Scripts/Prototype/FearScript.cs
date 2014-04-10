/// <summary>
/// Fear script.
/// 
/// this class is a singleton that handles what can collide with fears.
/// 
/// to call a function from this class, such as setIgnoreFears(Collider Other),
/// use FearScript.Instance.setIgnoreFears(Collider other) where it is needed.
/// 
/// to create a fear zone, add a empty gameobject with a collider for the area
/// desired. Tag the empty game object with appropriate type of fear. If this 
/// Fear zone is added at runtime, call reset fears function.
/// 
/// all fears ignores are set once at the begging. 
/// 
/// if you add another fear zone call the reset fears function.
/// 
/// if you add another object that needs to ignore fear zones, call the 
/// appropriate ignore fear zones or ignoreFears for all.
/// 
/// 
/// 
/// 4/9/2014
/// 	Changed darkness to a list.
/// 	When the light is triggered, now removes darkness from the list, instead of having zoey ignore the darkness.
/// 
/// </summary>



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FearScript : MonoBehaviour {

    List<GameObject> m_DarkFear = new List<GameObject>();
    GameObject[] m_ClaustrophobiaFear;
    GameObject[] m_HeightFear;
    GameObject[] m_Players;
    GameObject[] m_NerfDarts;
    GameObject[] m_Enemys;
    
	public static FearScript Instance{ get; private set; }
	

	
	/// <summary>
	/// used for singleton
	/// </summary>
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy this... there can be only one
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}
		
		//set the instance
		Instance = this;
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(gameObject);
	}

	/// <summary>
	/// fills all the arrays.
	/// calls the reset function to set fear ignores
	/// </summary>
	void Start ()
    {
				//calls a function that calls all the ignore collision voloumes
				resetCollisionIgnores ();
	}

	/// <summary>
	/// tells the collider to ignore the fears
	/// </summary>
	/// <param name="other">Other.</param>
	public void setIgnoreFears(Collider other)
    {
        setIgnoreDarkness(other);
        setIgnoreClaustrophobia(other);
		setIgnoreHeights (other);
    }

	/// <summary>
	/// ignores all fears, use this if gameobject has multiple colliders
	/// ( Character controllers are considered a collider ). 
	/// </summary>
	/// <param name="other">pass in the game object that needs to ignore fears.</param>
	public void setIgnoreFears(GameObject other)
	{
		Collider[] colliders = other.GetComponents<Collider> ();
		foreach(Collider col in colliders)
		{
			setIgnoreFears(col);
		}
	
	}

	/// <summary>
	/// ignores collision with heights 
	/// </summary>
	/// <param name="other">pass in the game object that needs to ignore fears.</param>
    public void setIgnoreHeights(Collider other)
    { 
        foreach (GameObject height in m_HeightFear)
        {
            //tells the colliders to ignore each other
            Physics.IgnoreCollision(other, height.collider);
        }
    
    }

	/// <summary>
	/// Sets the ignore darkness.
	/// </summary>
	/// <param name="other">pass in the game object that needs to ignore fears.</param>
    public void setIgnoreDarkness(Collider other)
    {
        
        //Loops through and set to ignore collision.
        foreach (GameObject dark in m_DarkFear)
        {
            //tells the colliders to ignore each other
            Physics.IgnoreCollision(other, dark.collider);
           // Physics.IgnoreCollision(dark.collider, other.collider, true);
        }
    
    }

	/// <summary>
	/// Sets the ignore claustrophobia.
	/// </summary>
	/// <param name="other">pass in the game object that needs to ignore fears.</param>
    public void setIgnoreClaustrophobia(Collider other)
    {
        foreach (GameObject claus in m_ClaustrophobiaFear)
        {
            //tells the colliders to ignore each other
            Physics.IgnoreCollision(other, claus.collider);
        }    
    }

	/// <summary>
	/// Sets the player ignore.
	/// </summary>
    public void setPlayerIgnore()
    {
        foreach (GameObject player in m_Players)
        {
            //Does a check to make sure only effects currect characters. 
            if (player.gameObject.name == "Derek")
            {
                Collider[] collider = player.GetComponents<Collider>();
                foreach (Collider col in collider)
                {
                    setIgnoreDarkness(col);
					setIgnoreHeights(col);
                }
            }

            if (player.gameObject.name == "Zoey")
            {
                Collider[] collider = player.GetComponents<Collider>();
                foreach (Collider col in collider)
                {
                    setIgnoreClaustrophobia(col);
					setIgnoreHeights(col);
                }
                
            }

            if (player.gameObject.name == "Alex")
            {
                Collider[] collider = player.GetComponents<Collider>();
                foreach (Collider col in collider)
                {
                    setIgnoreClaustrophobia( col );
					setIgnoreDarkness(col);
                }
                
            }
        
        }
    
    }

	/// <summary>
	/// Sets the nerf dart ignore.
	/// </summary>
    public void setNerfDartIgnore()
    {
        foreach (GameObject nerf in m_NerfDarts)
        {
            Collider[] collider = nerf.GetComponents<Collider>();
            foreach (Collider col in collider)
            {
                setIgnoreFears(col);  
            }
                 
        }   
    }

	/// <summary>
	/// Sets the enemy ignore.
	/// </summary>
    public void setEnemyIgnore()
    {
        foreach (GameObject enemy in m_Enemys)
        {
            Collider[] collider = enemy.GetComponents<Collider>();
            foreach (Collider col in collider)
            {
                setIgnoreFears(col);
            }
            
        }   
    }

	/// <summary>
	/// Resets the collision ignores.
	/// </summary>
    public void resetCollisionIgnores()
    {
		m_Players = GameObject.FindGameObjectsWithTag("Player");
		m_NerfDarts = GameObject.FindGameObjectsWithTag("NerfDart");
		m_DarkFear.AddRange (GameObject.FindGameObjectsWithTag("Darkness"));
		m_ClaustrophobiaFear = GameObject.FindGameObjectsWithTag("Claustrophobia");
		m_HeightFear = GameObject.FindGameObjectsWithTag("Height");
		m_Enemys = GameObject.FindGameObjectsWithTag("Enemy");		
		
		setPlayerIgnore();
        setNerfDartIgnore();
        setEnemyIgnore();
    }

	/// <summary>
	/// Removes the provided darkness fear gameObject.
	/// </summary>
	public void removeDarknessFear(GameObject darkness)
	{
		m_DarkFear.Remove(darkness);
	}

}
