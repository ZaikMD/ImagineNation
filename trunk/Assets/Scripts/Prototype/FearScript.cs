using UnityEngine;
using System.Collections;

public class FearScript : MonoBehaviour {

    GameObject[] m_DarkFear;
    GameObject[] m_ClaustrophobiaFear;
    GameObject[] m_HeightFear;
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
        m_HeightFear = GameObject.FindGameObjectsWithTag("Height");
        m_Enemys = GameObject.FindGameObjectsWithTag("Enemy");

        //calls a function that calls all the ignore collision voloumes
        resetCollisionIgnores();

	}
	
	// Update is called once per frame


    public void setIgnoreFears(Collider other)
    {
        setIgnoreDarkness(other);
        setIgnoreClaustrophobia(other);        
    }

    public void setIgnoreHeights(Collider other)
    { 
        foreach (GameObject height in m_HeightFear)
        {
            //tells the colliders to ignore each other
            Physics.IgnoreCollision(other, height.collider);
           // Physics.IgnoreCollision(dark.collider, other.collider, true);
        }
    
    }


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

    public void setIgnoreClaustrophobia(Collider other)
    {
        foreach (GameObject claus in m_ClaustrophobiaFear)
        {
            Debug.Log("ignoring alex");
            //tells the colliders to ignore each other
            Physics.IgnoreCollision(other, claus.collider);
            //Physics.IgnoreCollision( claus.collider, other.collider);
        }    
    }

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
                }
            }

            if (player.gameObject.name == "Zoey")
            {
                Collider[] collider = player.GetComponents<Collider>();
                foreach (Collider col in collider)
                {
                    setIgnoreClaustrophobia(col);
                }
                
            }

            if (player.gameObject.name == "Alex")
            {
                Collider[] collider = player.GetComponents<Collider>();
                foreach (Collider col in collider)
                {
                    setIgnoreHeights( col );
                }
                
            }
        
        }
    
    }
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

    public void resetCollisionIgnores()
    {
        setPlayerIgnore();
        setNerfDartIgnore();
        setEnemyIgnore();
    }

}
