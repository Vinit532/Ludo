using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Die : MonoBehaviour
{
    public static Die Instance;
    private PhotonView photonView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        // Rest of your code...
    }

    public void ClearInstancesForPlayer(int actorNumber)
    {
        photonView.RPC("ClearGeneratedInstancesRPC", RpcTarget.AllBuffered, actorNumber);
    }

    [PunRPC]
    private void ClearGeneratedInstancesRPC()
    {
        DieNumberSpawner dieNumberSpawner = FindObjectOfType<DieNumberSpawner>();
        if (dieNumberSpawner != null)
        {
            dieNumberSpawner.ClearGeneratedInstances();
        }
    }

    public void OnDieClicked()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("OnDieClickedRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void OnDieClickedRPC()
    {
        DieNumberSpawner dieNumberSpawner = FindObjectOfType<DieNumberSpawner>();

        if (dieNumberSpawner != null)
        {
            dieNumberSpawner.OnDieClicked();
        }
    }
}
