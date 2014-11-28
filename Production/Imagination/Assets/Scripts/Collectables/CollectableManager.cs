
/*Created by: Kole
 * 
 * This class was created to handle the spawning and keepig track 
 * of which of the light pegs and puzzled pieces that are active 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableManager : MonoBehaviour {

    Hud m_Hud;
 
    public GameObject m_LightPegPrefab;
	public GameObject m_PuzzlePiecePrefab;

    public GameObject[] m_LightPegsForCheckPointOne;
    public GameObject[] m_LightPegsForCheckPointTwo;
    public GameObject[] m_LightPegsForCheckPointThree;

	public GameObject[] m_PuzzlePieceForSection;
   
    bool[] m_LightPegCollected;
	bool[] m_PuzzlePieceCollected;

    const float OnScreenTime = 3;

    bool m_DisplayCounter;
    float m_Timer;
    int m_NumberOfLightPegsCollect;
	int m_NumberOfPuzzlePiecesCollected;

	// Use this for initialization
	void Start ()
    {
        m_Hud = GameObject.FindGameObjectWithTag(Constants.HUD).GetComponent<Hud>();

        m_Timer = OnScreenTime;
        m_NumberOfLightPegsCollect = 0;

        int lengthOfLightPegCollected = m_LightPegsForCheckPointOne.Length + m_LightPegsForCheckPointTwo.Length + m_LightPegsForCheckPointThree.Length;
		int lengthOPuzzlePieceCollected = m_PuzzlePieceForSection.Length;
        m_LightPegCollected = new bool[lengthOfLightPegCollected];
		m_PuzzlePieceCollected = new bool[lengthOPuzzlePieceCollected];

		short[] GameDataPuzzlePieceCollect = GameData.Instance.GetCollectedPuzzlePeices();
		
		for(int n = 0; n > GameDataPuzzlePieceCollect.Length; n++)
		{
			if(GameDataPuzzlePieceCollect[n] == 1)
			{
				m_PuzzlePieceCollected[n] = true; 
			}
			else
			{
				m_PuzzlePieceCollected[n] = false;
			}
		}

		SpawnLightPegs();
		SpawnPuzzlePieces();
	}
	
    // Update is called once per frame
	void Update () 
    {
        if (m_Timer > 0)
        {
            m_Timer -= Time.deltaTime;
        }
        else
        {
            m_DisplayCounter = false;
        }
    }

#region Spawn a light peg at random location

	/// <summary>
	/// Spawns the light peg at location.
	/// </summary>
	/// <param name="position">Position.</param>
	public void SpawnLightPegAtLocation(Vector3 position, int numberOfPegs)
	{
		for(int i = 0; i < numberOfPegs; i++)
		{
			GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);

			position.y += 0.5f;
			newLightPeg.transform.position = position;
			newLightPeg.AddComponent<EnemyLightPegSpawn> ();
		}
	}

	public void SpawnLightPegAtLocation(GameObject gameObjectSpawn, int numberOfPegs)
	{
		SpawnLightPegAtLocation(gameObjectSpawn.transform.position, numberOfPegs);	
	}

	public void SpawnLightPegAtLocation(Transform position, int numberOfPegs)
	{
		SpawnLightPegAtLocation(position.position, numberOfPegs);
	}

	public void SpawnLightPegAtLocation(MonoBehaviour Script, int numberOfPegs)
	{
		SpawnLightPegAtLocation (Script.transform.position, numberOfPegs);	
	}

#endregion

	void SpawnPuzzlePieces()
	{
		for(int i = 0; i < m_PuzzlePieceForSection.Length; i++)
		{
			if(!m_PuzzlePieceCollected[i])
			{
				GameObject newPuzzlePiece = (GameObject)Instantiate(m_PuzzlePiecePrefab);
				newPuzzlePiece.transform.position = m_PuzzlePieceForSection[i].transform.position;
				Destroy(m_PuzzlePieceForSection[i].gameObject);
				m_PuzzlePieceForSection[i] = newPuzzlePiece;
				newPuzzlePiece.GetComponent<PuzzlePiece>().SetInfo(i);
			}
		}
	}

	void SpawnLightPegs()
    {
        if (GameData.Instance.FirstTimePlayingLevel)
        {
            //first time the level is being played
            //We need to set gamedatas light pegs collected to all false
            GameData.Instance.SetCollectedPegs(m_LightPegCollected.Length);
			GameData.Instance.SetCollectedPuzzlePieces(m_PuzzlePieceCollected);
			GameData.Instance.FirstTimePlayingLevel = false;
        }
        else
        { 
            //Played this level before
            m_LightPegCollected = GameData.Instance.CollectedLightPegs();

			short[] temp = GameData.Instance.CollectedPuzzlePiece();
			m_PuzzlePieceCollected = new bool[temp.Length + 1];

			for (int i = 0; i < temp.Length; i++)
			{
				if(temp[i] == 1)
				{
					m_PuzzlePieceCollected[i] = true;   
				}
				else
				{
					m_PuzzlePieceCollected[i] = false;
				}
			}
        }

        //Check which CheckPoint we are starting at
        switch(GameData.Instance.CurrentCheckPoint)
        {
 
#region CheckPoint One
case CheckPoints.CheckPoint_1:
            //Loop through all list and spawn a new light peg
			for(int i = 0; i < m_LightPegsForCheckPointOne.Length; i++)
			{
				GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
				newLightPeg.transform.position = m_LightPegsForCheckPointOne[i].transform.position;
				Destroy(m_LightPegsForCheckPointOne[i].gameObject);
				m_LightPegsForCheckPointOne[i] = newLightPeg;
				newLightPeg.GetComponent<LightPeg>().SetInfo(i);
			}


			for(int i = 0; i < m_LightPegsForCheckPointTwo.Length; i++)
			{
				GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
				newLightPeg.transform.position = m_LightPegsForCheckPointTwo[i].transform.position;
				Destroy(m_LightPegsForCheckPointTwo[i].gameObject);
				m_LightPegsForCheckPointTwo[i] = newLightPeg;
				newLightPeg.GetComponent<LightPeg>().SetInfo(i + m_LightPegsForCheckPointOne.Length);
			}


			for(int i = 0; i < m_LightPegsForCheckPointThree.Length; i++)
			{
				GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
				newLightPeg.transform.position = m_LightPegsForCheckPointThree[i].transform.position;
				Destroy(m_LightPegsForCheckPointThree[i].gameObject);
				m_LightPegsForCheckPointThree[i] = newLightPeg;
				newLightPeg.GetComponent<LightPeg>().SetInfo(i + m_LightPegsForCheckPointOne.Length + m_LightPegsForCheckPointTwo.Length);
			}

            //Set counter to 0
            m_NumberOfLightPegsCollect = 0;
            break;
#endregion
#region CheckPoint Two         
case CheckPoints.CheckPoint_2:
            //loop through all after check point two and spawn, check if collected for checkpoint one
            //set counter to number of pegs collect in list for checkpoint one
            m_NumberOfLightPegsCollect = 0;

            for(int i = 0; i < m_LightPegsForCheckPointOne.Length; i++)
            {
                if (m_LightPegCollected[i])
                {
                    //This light peg is collected
                    //don't spawn
					//increment counter
                    Destroy(m_LightPegsForCheckPointOne[i].gameObject);
                    m_LightPegsForCheckPointOne[i] = null;
					m_NumberOfLightPegsCollect++;
                }
                else
                { 
                   //this light peg is not collected 
                   //Spawn light peg

                    GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
                    newLightPeg.transform.position = m_LightPegsForCheckPointOne[i].transform.position;
                    Destroy(m_LightPegsForCheckPointOne[i].gameObject);
                    m_LightPegsForCheckPointOne[i] = newLightPeg;
                }           
            }

            for (int i = 0; i < m_LightPegsForCheckPointTwo.Length; i++)
            {            
				GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
				newLightPeg.transform.position = m_LightPegsForCheckPointTwo[i].transform.position;
				Destroy(m_LightPegsForCheckPointTwo[i].gameObject);
				m_LightPegsForCheckPointTwo[i] = newLightPeg;
				newLightPeg.GetComponent<LightPeg>().SetInfo(i + m_LightPegsForCheckPointOne.Length);
				GameData.Instance.ResetCollectedPeg(i + m_LightPegsForCheckPointOne.Length);            
            }

			for(int i = 0; i < m_LightPegsForCheckPointThree.Length; i++)
			{
				GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
				newLightPeg.transform.position = m_LightPegsForCheckPointThree[i].transform.position;
				Destroy(m_LightPegsForCheckPointThree[i].gameObject);
				m_LightPegsForCheckPointThree[i] = newLightPeg;
				newLightPeg.GetComponent<LightPeg>().SetInfo(i + m_LightPegsForCheckPointOne.Length + m_LightPegsForCheckPointTwo.Length);
				GameData.Instance.ResetCollectedPeg(i + m_LightPegsForCheckPointOne.Length + m_LightPegsForCheckPointTwo.Length);  
			}
                break;
#endregion
#region CheckPoint Three
case CheckPoints.CheckPoint_3:
            //loop through all after check point three and spawn, check if collected for checkpoint one and two
            //set counter to number of pegs collect in list for checkpoint one and two

			m_NumberOfLightPegsCollect = 0;
			
			for(int i = 0; i < m_LightPegsForCheckPointOne.Length; i++)
			{
				if (m_LightPegCollected[i])
				{
					//This light peg is collected
					//don't spawn
					Destroy(m_LightPegsForCheckPointOne[i].gameObject);
					m_LightPegsForCheckPointOne[i] = null;
					m_NumberOfLightPegsCollect++;
				}
				else
				{ 
					//this light peg is not collected 
					//Spawn light peg					
					GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
					newLightPeg.transform.position = m_LightPegsForCheckPointOne[i].transform.position;
					Destroy(m_LightPegsForCheckPointOne[i].gameObject);
					m_LightPegsForCheckPointOne[i] = newLightPeg;
					newLightPeg.GetComponent<LightPeg>().SetInfo(i);
					GameData.Instance.ResetCollectedPeg(i);
				}           
			}


			for(int i = 0; i < m_LightPegsForCheckPointTwo.Length; i++)
			{
				if (m_LightPegCollected[i + m_LightPegsForCheckPointOne.Length])
				{
					//This light peg is collected
					//don't spawn
					Destroy(m_LightPegsForCheckPointOne[i].gameObject);
					m_LightPegsForCheckPointOne[i] = null;
					m_NumberOfLightPegsCollect++;
				}
				else
				{ 
					//this light peg is not collected 
					//Spawn light peg
					GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
					newLightPeg.transform.position = m_LightPegsForCheckPointTwo[i].transform.position;
					Destroy(m_LightPegsForCheckPointTwo[i].gameObject);
					m_LightPegsForCheckPointTwo[i] = newLightPeg;
					newLightPeg.GetComponent<LightPeg>().SetInfo(i + m_LightPegsForCheckPointOne.Length);
					GameData.Instance.ResetCollectedPeg(i + m_LightPegsForCheckPointOne.Length);      
				}           
			}

			//all after checkpoint there come back
			for(int i = 0; i < m_LightPegsForCheckPointThree.Length; i++)
			{
				GameObject newLightPeg = (GameObject)Instantiate(m_LightPegPrefab);
				newLightPeg.transform.position = m_LightPegsForCheckPointThree[i].transform.position;
				Destroy(m_LightPegsForCheckPointThree[i].gameObject);
				m_LightPegsForCheckPointThree[i] = newLightPeg;
				newLightPeg.GetComponent<LightPeg>().SetInfo(i + m_LightPegsForCheckPointOne.Length + m_LightPegsForCheckPointTwo.Length);
				GameData.Instance.ResetCollectedPeg(i + m_LightPegsForCheckPointOne.Length + m_LightPegsForCheckPointTwo.Length);  
			}

			break;
        
        }
#endregion

        //Cycle throught our list and spawn appropriate

    }

	void Load()
	{
		//GameData.Instance.GetCollectedPuzzlePeices();
	
	}

	void Save()
	{


	}

    public void IncrementCounter()
    {
        m_NumberOfLightPegsCollect ++;
        m_Hud.UpdateLightPegs(m_NumberOfLightPegsCollect);
    }

	public void IncrementPuzzleCounter()
	{
		m_NumberOfPuzzlePiecesCollected ++;
        
	}
	
	void DisplayCounter()
    {
        //change to referenec our hud, and call display hud
        m_DisplayCounter = true;
        m_Timer = OnScreenTime;
    }

    /// <summary>
    /// Displays the current amount of pegs and puzzle pieces collected
    /// </summary>
}
