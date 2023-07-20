using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private Dictionary<int, GameObject> playerManagerPrefabs = new Dictionary<int, GameObject>();
    

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        // Load all PlayerManager prefabs into the dictionary
        for (int i = 0; i < 4; i++) // Assuming a maximum of 4 players
        {
            string prefabName = "PlayerManager_" + i.ToString();
            GameObject playerManagerPrefab = Resources.Load<GameObject>(prefabName);
            if (playerManagerPrefab != null)
            {
                playerManagerPrefabs.Add(i, playerManagerPrefab);
            }
            else
            {
                Debug.LogError("PlayerManager prefab " + prefabName + " not found in the Resources folder.");
            }
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) // We're in the game scene
        {
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            // Instantiate the appropriate PlayerManager prefab for the local player
            if (playerManagerPrefabs.TryGetValue(playerIndex, out GameObject playerManagerPrefab))
            {
                PhotonNetwork.Instantiate(playerManagerPrefab.name, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.LogError("PlayerManager prefab for index " + playerIndex + " not found.");
            }

            // Instantiate TokenManager if it does not exist in the scene
            TokenManager tokenManager = FindObjectOfType<TokenManager>();
            if (tokenManager == null)
            {
                Instantiate(Resources.Load("TokenManager")); // Assumes TokenManager prefab is located in a "Resources" folder
            }
        }
    }

}