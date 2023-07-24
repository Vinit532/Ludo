using Photon.Pun;
using UnityEngine;

public class DieClickHandler : MonoBehaviour
{
    private DieController dieController;

    private void Awake()
    {
        dieController = GetComponentInParent<DieController>();
    }

    private void OnMouseDown()
    {
        if (dieController && dieController.gameObject.GetComponent<PhotonView>().IsMine)
        {
           // dieController.OnDieClick();
        }
    }
}
