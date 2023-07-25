using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class DieNumberSpawner : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject dieNumberPrefab;
    private static List<GameObject> generatedInstances = new List<GameObject>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDieClicked();
        }
    }

    public void OnDieClicked()
    {
        if (photonView.IsMine)
        {
            int randomNumber = Random.Range(1, 7);
            photonView.RPC("GenerateDieNumbers", RpcTarget.AllBuffered, randomNumber);
        }
    }

    [PunRPC]
    private void GenerateDieNumbers(int randomNumber)
    {
        // Clear all previously generated instances
        ClearGeneratedInstances();

        Debug.Log("Generated Number: " + randomNumber);

        for (int i = 0; i < randomNumber; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject instance = PhotonNetwork.Instantiate(dieNumberPrefab.name, Die.Instance.transform.position + spawnPosition, Quaternion.identity);
            instance.transform.SetParent(Die.Instance.transform); // Set the Die object as the parent
            generatedInstances.Add(instance);
        }

        Debug.Log("Number of Instances Created: " + randomNumber);
    }

    public void ClearGeneratedInstances()
    {
        foreach (var instance in generatedInstances)
        {
            PhotonNetwork.Destroy(instance);
        }
        generatedInstances.Clear();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Nothing to send over the network for this GameObject
        }
        else
        {
            // Nothing to receive from the network for this GameObject
        }
    }
}
