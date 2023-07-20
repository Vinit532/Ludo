using System.Collections.Generic;
using UnityEngine;

public class TokenSpawnManager : MonoBehaviour
{
    public static TokenSpawnManager Instance;

    // Dictionary to keep track of token prefabs for each block
    private Dictionary<string, List<GameObject>> tokenPrefabsByBlock = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // Method to add a token prefab to a specific block
    public void AddTokenPrefabToBlock(string blockName, GameObject tokenPrefab)
    {
        if (!tokenPrefabsByBlock.ContainsKey(blockName))
        {
            tokenPrefabsByBlock.Add(blockName, new List<GameObject>());
        }
        tokenPrefabsByBlock[blockName].Add(tokenPrefab);
    }

    // Method to place the token in the correct block
    public void PlaceTokenInBlock(string blockName, GameObject tokenPrefab)
    {
        if (tokenPrefabsByBlock.ContainsKey(blockName))
        {
            List<GameObject> tokenPrefabs = tokenPrefabsByBlock[blockName];
            foreach (GameObject prefab in tokenPrefabs)
            {
                if (prefab.name == tokenPrefab.name)
                {
                    Transform spawnpoint = GetNextAvailableSpawnpoint(blockName);
                    if (spawnpoint != null)
                    {
                        Instantiate(tokenPrefab, spawnpoint.position, spawnpoint.rotation);
                        break;
                    }
                }
            }
        }
    }

    // Method to get the next available spawn point for a block
    private Transform GetNextAvailableSpawnpoint(string blockName)
    {
        Transform[] spawnpoints = GameObject.Find(blockName).GetComponentsInChildren<Transform>();
        foreach (Transform spawnpoint in spawnpoints)
        {
            if (spawnpoint.childCount == 0)
            {
                return spawnpoint;
            }
        }
        return null;
    }
}
