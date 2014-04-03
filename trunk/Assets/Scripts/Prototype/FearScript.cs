using UnityEngine;
using System.Collections;

public class FearScript : MonoBehaviour {

    GameObject[] m_DarkFear;
    GameObject[] m_ClaustrophobiaFear;
    GameObject[] m_Players;
    GameObject[] m_NerfDarts;
    GameObject[] m_Enemys;
    
	public static FearScript Instance{ get; private set; }
	

	
	
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






	void Start ()
    {

        m_Players = GameObject.FindGameObjectsWithTag("Player");
		m_NerfDarts = GameObject.FindGameObjectsWithTag("NerfDart");
        m_DarkFear = GameObject.FindGameObjectsWithTag("Darkness");
        m_ClaustrophobiaFear = GameObject.FindGameObjectsWithTag("Claustrophobia");
        m_Enemys = GameObject.FindGameObjectsWithTag("Enemy");

        //calls a function that calls all the ignore collision voloumes
        resetCollisionIgnores();

	}
	
	// Update is called once per frame


    public void setIgnoreFears(GameObject other)
    {
        setIgnoreDarkness(other);
        setIgnoreClaustrophobia(other);        
    }

    public void setIgnoreDarkness(GameObject other)
    {
        //Loops through and set to ignore collision.
        foreach (GameObject dark in m_DarkFear)
        {
            //tells the colliders to ignore each other
            Physics.IgnoreCollision(other.collider, dark.collider);
        }
    
    }

    public void setIgnoreClaustrophobia(GameObject other)
    {
        foreach (GameObject claus in m_ClaustrophobiaFear)
        {
            //tells the colliders to ignore each other
            Physics.IgnoreCollision(other.collider, claus.collider);
        }    
    }

    public void setPlayerIgnore()
    {
        foreach (GameObject player in m_Players)
        {
            //Does a check to make sure only effects currect characters. 
            if (player.gameObject.name == "Derek")
            {
                setIgnoreDarkness(player.gameObject);
            }

            if (player.gameObject.name == "Zoey")
            {
                setIgnoreClaustrophobia(player.gameObject);
            }

            if (player.gameObject.name == "Alex")
            {
                setIgnoreFears(player.gameObject);
            }
        
        }
    
    }
    public void setNerfDartIgnore()
    {
        foreach (GameObject nerf in m_NerfDarts)
        {
            setIgnoreFears(nerf.gameObject);       
        }   
    }

    public void setEnemyIgnore()
    {
        foreach (GameObject enemy in m_Enemys)
        {
            setIgnoreFears(enemy.gameObject);
        }   
    }

    public void resetCollisionIgnores()
    {
        setPlayerIgnore();
        setNerfDartIgnore();
        setEnemyIgnore();
    }

}
