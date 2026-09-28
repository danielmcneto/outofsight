using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAnimate : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 100f; // Degrees per second

    [Header("Bobbing Settings")]
    public float bobbingHeight = 0.25f; // How high and low it moves
    public float bobbingSpeed = 3f;     // Movement frequency

    private Vector3 initialPosition;

    private void Start()
    {
        // Store the original position as a baseline for the bobbing motion
        initialPosition = transform.position;
    }

    private void Update()
    {
        AnimateCollectible();
    }

    private void AnimateCollectible()
    {
        // 1. Continuous rotation around the world Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // 2. Smooth sine wave oscillation for the floating effect
        float newYOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;

        // Apply the new position relative to the starting point
        transform.position = initialPosition + new Vector3(0f, newYOffset, 0f);
    }
}
