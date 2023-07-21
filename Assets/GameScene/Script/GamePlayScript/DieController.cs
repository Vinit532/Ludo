using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DieController : MonoBehaviour, IPointerClickHandler
{
    public GameObject dieNumberPrefab;
    private List<GameObject> generatedInstances = new List<GameObject>();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPress == gameObject)
        {
            GenerateDieNumbers();
        }
    }

    private void GenerateDieNumbers()
    {
        // Clear previously generated instances
        ClearGeneratedInstances();

        // Generate a random number between 1 and 6
        int randomNumber = Random.Range(1, 7);
        Debug.Log("Generated Number: " + randomNumber);

        // Create instances of the dieNumberPrefab based on the generated number
        for (int i = 0; i < randomNumber; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject instance = Instantiate(dieNumberPrefab, transform.position + spawnPosition, Quaternion.identity, transform);
            instance.name = "DieNumber_" + i;
            generatedInstances.Add(instance);
        }

        Debug.Log("Number of Instances Created: " + randomNumber);
    }

    private void ClearGeneratedInstances()
    {
        foreach (var instance in generatedInstances)
        {
            Destroy(instance);
        }
        generatedInstances.Clear();
    }
}
