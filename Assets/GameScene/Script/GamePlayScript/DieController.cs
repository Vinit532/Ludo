using UnityEngine;

public class DieController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Roll()
    {
        // Generate a random rotation for the die
        Quaternion randomRotation = Random.rotation;

        // Apply the rotation to the die
        rb.rotation = randomRotation;

        // Add a force to the die to simulate rolling
        Vector3 rollForce = randomRotation * Vector3.forward * 10f;
        rb.AddForce(rollForce, ForceMode.Impulse);
    }
}
