using UnityEngine;
using UnityEditor;
using System.Collections;

public class InitializeScene : EditorWindow 
{
    const string GROUND_NAME = "Ground";
    const string MANAGERS_NAME = "Managers";
    const string PLAYER_SPAWNER_NAME = "PlayerSpawner  DO NOT ADD TO THIS";
    const string CHECK_POINT_NAME = "Checkpoint";

    //const string CHECKPOINT_PREFAB_PATH = "Assets/Prefabs/CheckpointPrefab.prefab";
    const string CHECKPOINT_PREFAB_PATH = "test/Ground";

    [MenuItem("Tools/InitializeScene")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(InitializeScene));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Initialize Scene (warning will delete old scene)"))
        {
            clearScene();
            buildScene();
        }     
    }

    void clearScene()
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject.DestroyImmediate((GameObject)o);
        }
    }

    void buildScene()
    {
        //land(ground)
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = GROUND_NAME;
        ground.transform.position = new Vector3();

        GameObject managers = new GameObject();
        managers.name = MANAGERS_NAME;
        managers.AddComponent<GameData>();//gamedata
        managers.AddComponent<TESTSetGameData>();//set gamedata
        managers.AddComponent<SFXManager>();//sound manager

        GameObject playerSpawner = new GameObject();
        playerSpawner.name = PLAYER_SPAWNER_NAME;
        playerSpawner.AddComponent<PlayerSpawner>();//player spawner

        Object loaded = Resources.Load(CHECKPOINT_PREFAB_PATH);
        GameObject test = (GameObject)Instantiate(loaded);
       //GameObject.Instantiate(checkpoint);
        //checkpoint.name = CHECK_POINT_NAME;
        //checkpoint.transform.position = new Vector3(0.0f, 2.5f, 0.0f);

        //CheckPoint checkpointScript = checkpoint.AddComponent<CheckPoint>();
        //checkpointScript.m_OnMaterial = new 

        GameObject[] playerSpawns = new GameObject[2];


        //dead player manager /grave
        
        //check Point
        
        //pause screen
    }
}
