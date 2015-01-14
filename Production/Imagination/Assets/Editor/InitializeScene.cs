using UnityEngine;
using UnityEditor;
using System.Collections;

public class InitializeScene : EditorWindow 
{
    const string GROUND_NAME = "Ground";
    const string MANAGERS_NAME = "Managers";
    const string PLAYER_SPAWNER_NAME = "PlayerSpawner  DO NOT ADD TO THIS";
    const string CHECK_POINT_NAME = "Checkpoint";

    const string CHECKPOINT_PREFAB_PATH = "Prefabs/Interactables/Check Point";

	const string ALEX_PREFAB_PATH = "Prefabs/Players/Alex";
	const string DEREK_PREFAB_PATH = "Prefabs/Players/Derek";
	const string ZOE_PREFAB_PATH = "Prefabs/Players/Zoe";

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
		PlayerSpawner spawner = playerSpawner.AddComponent<PlayerSpawner>();//player spawner

		Object loaded = Resources.Load(ALEX_PREFAB_PATH);
		spawner.AlexPrefab = (GameObject)loaded;

		loaded = Resources.Load(DEREK_PREFAB_PATH);
		spawner.DerekPrefab = (GameObject)loaded;

		loaded = Resources.Load(ZOE_PREFAB_PATH);
		spawner.ZoeyPrefab = (GameObject)loaded;


        loaded = Resources.Load(CHECKPOINT_PREFAB_PATH);
        GameObject test = (GameObject)Instantiate(loaded);
		test.transform.position = Vector3.zero;


    }
}
