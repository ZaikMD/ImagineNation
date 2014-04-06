/// <summary>
/// 
/// THIS CLASS IS FOR FUTURE VERSIONS 
/// 
/// this class is for use menu and keeping track of setting, 
/// it will be used when implementing multipule scenes( Next Semester)
/// 
/// Setting.
/// 
/// </summary>




//using UnityEngine;
//using System.Collections;
//
//public class Setting : MonoBehaviour {
//
//    public static Setting Instance { get; private set; }
//    // Use this for initialization
//
//    void Awake()
//    {
//        //if theres another instance (there shouldnt be) destroy it... there can be only one
//        if (Instance != null && Instance != this)
//        {
//            //destroy all other instances
//            Destroy(gameObject);
//        }
//
//        //set the instance
//        Instance = this;
//
//        //prevents this object being destroyed between scene loads
//        DontDestroyOnLoad(gameObject);
//    }
//
//    public Player m_PlayerOneSelected;
//    public Player m_PlayerTwoSelected;
//    Camera m_PlayerCamera;
//    public GameObject m_CheckPoint;
//
//    public GameObject PlayerOne;
//    public GameObject PlayerTwo;
//
//	// Use this for initialization
//	void Start ()
//    {
//	
//	}
//	
//	// Update is called once per frame
//	void Update ()
//    {
//	
//	}
//
//    public void SetPlayer()
//    {
//        switch (m_PlayerOneSelected)
//        {
//            case Player.Alex:
//                {
//                    PlayerOne = GameObject.Find("Alex");
//                    break;
//                }
//
//            case Player.Derek:
//                {
//                    PlayerOne = GameObject.Find("Derek");
//                    break;
//                }
//
//            case Player.Zoey:
//                {
//                    PlayerOne = GameObject.Find("Zoey");
//                    break;
//                }
//        }
//
//        switch (m_PlayerTwoSelected)
//        {
//            case Player.Alex:
//                {
//                    PlayerTwo = GameObject.Find("Alex");
//                    break;
//                }
//
//            case Player.Derek:
//                {
//                    PlayerTwo = GameObject.Find("Derek");
//                    break;
//                }
//
//            case Player.Zoey:
//                {
//                    PlayerTwo = GameObject.Find("Zoey");
//                    break;
//                }
//        }
//
//
//
//      //  PlayerOne.SetActive(true);
//      //  PlayerTwo.SetActive(true);
//
//     //   PlayerTwo.GetComponent<PlayerAIStateMachine>().enabled = true;
//
//    }
//
//
//}
