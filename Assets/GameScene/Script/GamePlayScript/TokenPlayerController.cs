using UnityEngine;
using Photon.Pun;

public class TokenPlayerController : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
        {
            // Disable components not belonging to the local player
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
