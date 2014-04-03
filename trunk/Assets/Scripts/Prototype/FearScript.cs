using UnityEngine;
using System.Collections;

public class FearScript : MonoBehaviour {

    GameObject[] m_DarkFear;
    GameObject[] m_ClaustrophobiaFear;
    GameObject[] m_Players;
    GameObject[] m_NerfDarts;
    GameObject[] m_Enemys;
    	
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
	void Update ()
    {
	
	}

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
            if (this.gameObject.name == "Derek")
            {
                setIgnoreDarkness(player.gameObject);
            }

            if (this.gameObject.name == "Zoey")
            {
                setIgnoreClaustrophobia(player.gameObject);
            }

            if (this.gameObject.name == "Alex")
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
