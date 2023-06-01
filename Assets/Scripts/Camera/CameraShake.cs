using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{


    public CameraFollow follow;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;
    public float dampingSpeed = 2.0f;

    private Vector3 initialPosition;
    private float currentShakeDuration = 0.0f;

    private void Start()
    {
     
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (currentShakeDuration > 0)
        {
            // Generate a random offset within a range for the camera position
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;

            // Apply the offset to the camera position
            transform.localPosition = initialPosition + randomOffset;

            // Reduce the shake duration over time based on the damping speed
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // Reset the camera position to its initial position
            follow.CameraShakeFalse();
            transform.localPosition = initialPosition;
            this.enabled = false;
        }
    }

    public void ShakeCamera()
    {
        currentShakeDuration = shakeDuration;
    }
}
