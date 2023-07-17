using UnityEngine;

public class FPS_Player : MonoBehaviour
{
    public GameObject playerPrefab; // Assign the player prefab in the Inspector

    public float moveSpeed = 5f; // Adjust as needed
    public float mouseSensitivity = 2f; // Adjust as needed

    private float verticalRotation = 0f;
    private CharacterController controller;
    private Camera playerCamera;

    private void Awake()
    {
        controller = playerPrefab.GetComponent<CharacterController>();
        controller.enabled = false; // Disable the controller initially
    }

    private void Start()
    {
        // Instantiate the player prefab at the PlayerSpawnPoint position and rotation
        GameObject player = Instantiate(playerPrefab, transform.position, transform.rotation);
        // Set the instantiated player as a child of the empty GameObject
        player.transform.parent = transform;
        // Get the Camera component from the player and store it for rotation
        playerCamera = player.GetComponentInChildren<Camera>(); // Assign playerCamera here
        controller = player.GetComponent<CharacterController>(); // Assign the controller after instantiation
    }

    private void Update()
    {
        // Check if the controller is active before moving
        if (!controller.enabled)
        {
            controller.enabled = true; // Enable the controller before moving
        }

        // Handle movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveHorizontal, 0f, moveVertical));
        moveDirection *= moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);

        // Handle rotation
        float mouseMoveX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseMoveY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseMoveY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, mouseMoveX, 0f);
    }
}
